using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TrainineeAPI.DTOs;
using TrainineeAPI.Models;

public class SubmissionService : ISubmissionService
{
    private readonly TraineeContext _traineeContext;
    private readonly IMapper _mapper;

    public SubmissionService(TraineeContext traineeContext,IMapper mapper)
    {
        _traineeContext = traineeContext;
        _mapper = mapper;
    }

    public async Task<SubmissionDto> Create(CreateSubmissionDto request)
    {
        var id = _traineeContext.Submissions.Count() == 0 ? 1 : _traineeContext.Submissions.Max(s => s.Id + 1);

        // validate taskassignment document exist
        TaskAssignment taskAssignment = _traineeContext.TaskAssignments.FirstOrDefault(t => t.Id == request.TaskAssignmentId)!;

        if (taskAssignment == null)
        {
            Console.WriteLine("Task Assignment doc not found");
            return null;
        }

        Submission submission = new Submission
        {
            Id = id,
            TaskAssignmentId = request.TaskAssignmentId,
            SubmissionUrl = request.SubmissionUrl,
            Notes = request.Notes!,
            Status = request.Status!,
            SubmittedDate = DateOnly.FromDateTime(DateTime.Now),
            CreatedDate = DateOnly.FromDateTime(DateTime.Now),
            UpdatedDate = DateOnly.FromDateTime(DateTime.Now),
        };

        _traineeContext.Submissions.Add(submission);
        await _traineeContext.SaveChangesAsync();

        SubmissionDto submissionDto = _mapper.Map<SubmissionDto>(submission);

        return submissionDto;
    }

    public async Task<List<SubmissionDto>> GetAll()
    {
        var submissions = await _traineeContext.Submissions.ToListAsync();

        var submissionDtos = submissions
            .Select(t => _mapper.Map<SubmissionDto>(t))
            .ToList();

        return submissionDtos;
    }

    public SubmissionDto? GetById(int id)
    {
        var submissionById = _traineeContext.Submissions.FirstOrDefault(t => t.Id == id);

        if (submissionById == null)
        {
            return null;
        }

        SubmissionDto submissionDtos = _mapper.Map<SubmissionDto>(submissionById);

        return submissionDtos;
    }
}