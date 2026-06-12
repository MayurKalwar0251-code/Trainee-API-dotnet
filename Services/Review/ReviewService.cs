using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TrainineeAPI.DTOs;
using TrainineeAPI.Models;

public class ReviewService : IReviewService
{
    private readonly TraineeContext _traineeContext;
    private readonly IMapper _mapper;

    public ReviewService(TraineeContext traineeContext, IMapper mapper)
    {
        _traineeContext = traineeContext;
        _mapper = mapper;
    }

    public async Task<ServiceResult<ReviewDto>> Create(CreateReviewDto request)
    {
        // validate submissionId,mentorId check if exist
        Submission submission = _traineeContext.Submissions.FirstOrDefault(s => s.Id == request.SubmissionId)!;
        Mentor mentor = _traineeContext.Mentors.FirstOrDefault(s => s.Id == request.MentorId)!;

        if (submission == null || mentor == null)
        {
            Console.WriteLine("Submission Doc or Mentor Doc doesnt exist");
            return ServiceResult<ReviewDto>.Fail("Submission Doc or Mentor Doc doesnt exist");
        }

        var Id = _traineeContext.Reviews.Count() == 0 ? 1 : _traineeContext.Reviews.Max(r => r.Id + 1);

        Review review = new Review
        {
            Id = Id,
            SubmissionId = request.SubmissionId,
            MentorId = request.MentorId,
            Feedback = request.Feedback,
            Score = request.Score,
            ReviewStatus = request.ReviewStatus,
            ReviewedDate = DateOnly.FromDateTime(DateTime.Now),
            CreatedDate = DateOnly.FromDateTime(DateTime.Now),
            UpdatedDate = DateOnly.FromDateTime(DateTime.Now),
        };

        _traineeContext.Reviews.Add(review);
        await _traineeContext.SaveChangesAsync();

        ReviewDto reviewDto = _mapper.Map<ReviewDto>(review);
        return ServiceResult<ReviewDto>.Ok(reviewDto);
    }

    public async Task<ServiceResult<List<ReviewDto>>> GetAll()
    {
        var reviews = await _traineeContext.Reviews.ToListAsync();

        var reviewDtos = reviews
            .Select(r => _mapper.Map<ReviewDto>(r))
            .ToList();

        return ServiceResult<List<ReviewDto>>.Ok(reviewDtos);
    }

    public ServiceResult<ReviewDto> GetById(int id)
    {
        var reviewById = _traineeContext.Reviews.FirstOrDefault(r => r.Id == id);

        if (reviewById == null)
        {
            return ServiceResult<ReviewDto>.Fail(ErrorConstants.DocumentNotFound);
        }

        ReviewDto reviewDtos = _mapper.Map<ReviewDto>(reviewById);

        return ServiceResult<ReviewDto>.Ok(reviewDtos);
    }
}