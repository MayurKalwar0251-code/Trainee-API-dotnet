using AutoMapper;
using TrainineeAPI.DTOs;
using TrainineeAPI.Models;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Maps properties with identical names automatically
        CreateMap<User, UserResponseDto>().ReverseMap();
        CreateMap<Trainee, TraineeDto>().ReverseMap();
        CreateMap<Trainee, CreateTraineeDto>().ReverseMap();
        CreateMap<Trainee, UpdateTraineeDto>().ReverseMap();
        CreateMap<Mentor, MentorDto>().ReverseMap();
        CreateMap<Mentor, CreateMentorDto>().ReverseMap();
        CreateMap<Mentor, UpdateMentorDto>().ReverseMap();
        CreateMap<LearningTask, LearningTaskDto>().ReverseMap();
        CreateMap<LearningTask, CreateLearningTaskDto>().ReverseMap();
        CreateMap<LearningTask, UpdateLearningTaskDto>().ReverseMap();
        CreateMap<TaskAssignment, TaskAssignmentDto>().ReverseMap();
        CreateMap<TaskAssignment, CreateTaskAssignmentDto>().ReverseMap();
        CreateMap<TaskAssignment, UpdateTaskAssignmentDto>().ReverseMap();
        CreateMap<Submission, SubmissionDto>().ReverseMap();
        CreateMap<Submission, CreateSubmissionDto>().ReverseMap();
        CreateMap<Review, ReviewDto>().ReverseMap();
        CreateMap<Review, CreateReviewDto>().ReverseMap();
    }
}
