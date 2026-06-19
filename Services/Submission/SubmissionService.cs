using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using TrainineeAPI.DTOs;
using TrainineeAPI.Models;
using System.Security.Claims;

public class SubmissionService : ISubmissionService
{
    private readonly TraineeContext _traineeContext;
    private readonly IMapper _mapper;

    private readonly ILocalFileStorage _localFileStorage;
    private readonly IHttpContextAccessor _httpContextAccessor;
    
    public SubmissionService(TraineeContext traineeContext, IMapper mapper, ILocalFileStorage localFileStorage, IHttpContextAccessor httpContextAccessor)
    {
        _traineeContext = traineeContext;
        _mapper = mapper;
        _localFileStorage = localFileStorage;
        _httpContextAccessor = httpContextAccessor;
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

    public ServiceResult<SubmissionDto> GetById(int id)
    {
        var submissionById = _traineeContext.Submissions.FirstOrDefault(t => t.Id == id);

        if (submissionById == null)
        {
            return ServiceResult<SubmissionDto>.Fail(ErrorConstants.DocumentNotFound);
        }

        SubmissionDto submissionDtos = _mapper.Map<SubmissionDto>(submissionById);

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
            await _traineeContext.SaveChangesAsync();
        }


        return ServiceResult<IEnumerable<SubmissionFile>>.Ok(submissionFiles);
    }
}