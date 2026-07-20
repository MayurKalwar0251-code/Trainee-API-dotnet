using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using TrainineeAPI.DTOs;
using TrainineeAPI.Models;
using System.Security.Claims;
using Microsoft.Extensions.Caching.Distributed;

public class SubmissionService : ISubmissionService
{
    private readonly TraineeContext _traineeContext;
    private readonly IMapper _mapper;

    private readonly ILocalFileStorage _localFileStorage;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IConfiguration _configuration;
    private readonly ICacheService _cacheService;
    private readonly IRabbitMQPublisher _rabbitMQPublisher;
    
    public SubmissionService(TraineeContext traineeContext, IMapper mapper, ILocalFileStorage localFileStorage, IHttpContextAccessor httpContextAccessor, IConfiguration configuration,ICacheService cacheService,IRabbitMQPublisher rabbitMQPublisher)
    {
        _traineeContext = traineeContext;
        _mapper = mapper;
        _localFileStorage = localFileStorage;
        _httpContextAccessor = httpContextAccessor;
        _configuration = configuration;
        _cacheService = cacheService;
        _rabbitMQPublisher = rabbitMQPublisher;
    }

    public async Task<ServiceResult<SubmissionDto>> Create(CreateSubmissionDto request)
    {
        var id = _traineeContext.Submissions.Count() == 0 ? 1 : _traineeContext.Submissions.Max(s => s.Id + 1);

        // validate taskassignment document exist
        TaskAssignment taskAssignment = _traineeContext.TaskAssignments.FirstOrDefault(t => t.Id == request.TaskAssignmentId)!;

        if (taskAssignment == null)
        {
            Console.WriteLine("Task Assignment doc not found");
            return ServiceResult<SubmissionDto>.Fail(ErrorConstants.DocumentNotFound);
        }

        Submission submission = _mapper.Map<Submission>(request);
        submission.Id = id;
        submission.SubmittedDate = DateOnly.FromDateTime(DateTime.Now);
        submission.CreatedDate = DateOnly.FromDateTime(DateTime.Now);
        submission.UpdatedDate = DateOnly.FromDateTime(DateTime.Now);

        _traineeContext.Submissions.Add(submission);
        await _traineeContext.SaveChangesAsync();

        SubmissionDto submissionDto = _mapper.Map<SubmissionDto>(submission);

        return ServiceResult<SubmissionDto>.Ok(submissionDto);
    }

    public async Task<ServiceResult<List<SubmissionDto>>> GetAll()
    {
        var submissions = await _traineeContext.Submissions.ToListAsync();

        var submissionDtos = submissions
            .Select(t => _mapper.Map<SubmissionDto>(t))
            .ToList();

        return ServiceResult<List<SubmissionDto>>.Ok(submissionDtos);

    }

    public async Task<ServiceResult<SubmissionDto>> GetById(int id)
    {
        string key = $"submission:{id}";

        var data = await _cacheService.GetAsync<SubmissionDto>(key);

        if (data != null)
        {
            Console.WriteLine("Fetched from Cache : " + key);
            return ServiceResult<SubmissionDto>.Ok(data);
        }

        Console.WriteLine("Fetching from db : " + key);

        var submissionById = _traineeContext.Submissions.FirstOrDefault(t => t.Id == id);

        if (submissionById == null)
        {
            return ServiceResult<SubmissionDto>.Fail(ErrorConstants.DocumentNotFound);
        }

        SubmissionDto submissionDtos = _mapper.Map<SubmissionDto>(submissionById);

        var cacheOptions = new DistributedCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(20));
        await _cacheService.SetAsync(key,submissionDtos,cacheOptions);

        return ServiceResult<SubmissionDto>.Ok(submissionDtos);

    }

    public async Task<ServiceResult<IEnumerable<SubmissionFile>>> SubmitSubmissionFile(int submissionId,SubmitSubmissionFileDto submit)
    {
        Console.WriteLine("Submission submit service function");
        Submission submission = _traineeContext.Submissions.FirstOrDefault(s=> s.Id == submissionId)!;

        if (submission == null)
        {
            return ServiceResult<IEnumerable<SubmissionFile>>.Fail(ErrorConstants.DocumentNotFound);
        }

        // check length of files > 0
        if (submit.Files.Count == 0)
        {
            return ServiceResult<IEnumerable<SubmissionFile>>.Fail("Upload Atleast one file");
        }

        var uploadedFiles = await _localFileStorage.SaveAsync(submit.Files);

        Console.WriteLine("Uploaded files ");
        Console.WriteLine(uploadedFiles);

        List<SubmissionFile> submissionFiles = [];

        foreach (var file in uploadedFiles.Data!)
        {
            var id = _traineeContext.SubmissionFiles.Count() == 0 ? 1 : _traineeContext.SubmissionFiles.Max(i => i.Id) + 1;

            var userId = int.Parse(_httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);

            Console.Write("USER ID : " + userId);

            SubmissionFile submissionFile = new SubmissionFile
            {
                Id = id,
                SubmissionId = submission.Id,
                UserId = userId,
                OriginalFileName = file.OriginalFileName,
                GeneratedStorageName = file.GeneratedStorageName,
                ContentType = file.ContentType,
                Checksum = file.Checksum,
                Size = file.Size,
                CreatedDate = DateOnly.FromDateTime(DateTime.Now),
                UpdatedDate = DateOnly.FromDateTime(DateTime.Now),
            };

            submissionFiles.Add(submissionFile);

            _traineeContext.SubmissionFiles.Add(submissionFile);
            
            // publish message
            SubmissionProcessingRequestModel submissionProcessingRequestModel = new SubmissionProcessingRequestModel
            {
                CorrelationId = Guid.NewGuid().ToString(),
                MessageId = Guid.NewGuid().ToString(),
                RequestedAt = DateTime.UtcNow,
                SubmissionId = submission.Id,
                SubmissionFileId = submissionFile.Id,
            };

            // create processing job and add in database
            ProcessingJob processingJob = new ProcessingJob
            {
                CorrelationId = submissionProcessingRequestModel.CorrelationId,  
                MessageId = submissionProcessingRequestModel.MessageId,  
                Status = "Queued",
                SubmissionFileId = submissionFile.Id,
                Attempts = 0,
                Id = _traineeContext.ProcessingJobs.Count() == 0 ? 1 : _traineeContext.ProcessingJobs.Max(i => i.Id) + 1,
                StartedTime = DateTime.UtcNow,
                CompletedTime = DateTime.UtcNow,
            };

            _traineeContext.ProcessingJobs.Add(processingJob);
            await _traineeContext.SaveChangesAsync();

            Console.WriteLine("Publishing message in rabbitMq");
            await _rabbitMQPublisher.PublishMessageAsync(submissionProcessingRequestModel, RabbitMQQueues.SubmissionProcessingQueue);
            Console.WriteLine("Published message in rabbitMq");
        }


        return ServiceResult<IEnumerable<SubmissionFile>>.Ok(submissionFiles);
    }
    public async Task<ServiceResult<GetFileResponseDto>> DownloadFile(int id)
    {
        Console.WriteLine("Download File Service FUnction");
        // check submission file exists
        SubmissionFile submissionFile = _traineeContext.SubmissionFiles.FirstOrDefault(s => s.Id == id)!;

        if (submissionFile == null)
        {
            return ServiceResult<GetFileResponseDto>.Fail(ErrorConstants.DocumentNotFound);
        }

        Console.WriteLine("Service File Found");

        // check if file exists in storage
        string GeneratedStorageName = submissionFile.GeneratedStorageName;
        string basePath = _configuration["StoredFilesPath"]!;
        string path = Path.Combine(basePath,GeneratedStorageName);

        Console.WriteLine("File path " + path);
        if (File.Exists(path) == false)
        {
            return ServiceResult<GetFileResponseDto>.Fail("File Not Found");
        }

        Console.WriteLine("File exists");

        // call localstorage.OpenReadAsync() function to read as bytes
        var readAsyncBytes = await _localFileStorage.OpenReadAsync(path);

        Console.WriteLine("Reading file bytes " + readAsyncBytes);

        GetFileResponseDto getFileResponseDto = new GetFileResponseDto
        {
            FileByte = readAsyncBytes.Data!,
            ContentType = submissionFile.ContentType,
            fileDownloadName = submissionFile.GeneratedStorageName
        };

        return ServiceResult<GetFileResponseDto>.Ok(getFileResponseDto);
    }

    public async Task<ServiceResult<string>> DeleteFile(int id)
    {
        Console.WriteLine("Download File Service FUnction");
        // check submission file exists
        SubmissionFile submissionFile = _traineeContext.SubmissionFiles.FirstOrDefault(s => s.Id == id)!;

        if (submissionFile == null)
        {
            return ServiceResult<string>.Fail(ErrorConstants.DocumentNotFound);
        }

        Console.WriteLine("Service File Found");

        // check if file exists in storage
        string GeneratedStorageName = submissionFile.GeneratedStorageName;
        string basePath = _configuration["StoredFilesPath"]!;
        string path = Path.Combine(basePath,GeneratedStorageName);

        Console.WriteLine("File path " + path);
        if (File.Exists(path) == false)
        {
            return ServiceResult<string>.Fail("File Not Found");
        }

        Console.WriteLine("File exists");

        // call localstorage function to delete and delete submission file from db

        _localFileStorage.DeleteAsync(path);

        _traineeContext.SubmissionFiles.Remove(submissionFile);
        await _traineeContext.SaveChangesAsync();

        return ServiceResult<string>.Ok("Deleted File Successfully");
    }
}