using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TrainineeAPI.DTOs;
using TrainineeAPI.Models;

public class SubmissionService : ISubmissionService
{
    private readonly TraineeContext _traineeContext;
    private readonly IMapper _mapper;

    public SubmissionService(TraineeContext traineeContext, IMapper mapper)
    {
        _traineeContext = traineeContext;
        _mapper = mapper;
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
}