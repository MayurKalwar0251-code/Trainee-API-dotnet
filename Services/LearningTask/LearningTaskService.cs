using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TrainineeAPI.DTOs;
using TrainineeAPI.Models;

public class LearningTaskService : ILearningTaskService
{
    private readonly TraineeContext _traineeContext;
    private readonly IMapper _mapper;

    public LearningTaskService(TraineeContext traineeContext,IMapper mapper)
    {
        _traineeContext = traineeContext;
        _mapper = mapper;
    }

    public async Task<ServiceResult<LearningTaskDto>> Create(CreateLearningTaskDto learningTask)
    {
        var id = _traineeContext.LearningTasks.Count() == 0 ? 1 : _traineeContext.LearningTasks.Max(t => t.Id) + 1;

        LearningTask newLearningTask = new LearningTask
        {
            Id = id,
            Title = learningTask.Title!,
            Description = learningTask.Description!,
            ExpectedTechStack = learningTask.ExpectedTechStack!,
            DueDate = learningTask.DueDate,
            Status = learningTask.Status,
            CreatedDate = DateOnly.FromDateTime(DateTime.Now),
            UpdatedDate = DateOnly.FromDateTime(DateTime.Now),
        };

        LearningTaskDto learningTaskDto = _mapper.Map<LearningTaskDto>(newLearningTask);

        _traineeContext.LearningTasks.Add(newLearningTask);

        await _traineeContext.SaveChangesAsync();

        return ServiceResult<LearningTaskDto>.Ok(learningTaskDto);
    }

    public async Task<ServiceResult<bool>> Delete(int id)
    {
        var learningTask = _traineeContext.LearningTasks.FirstOrDefault(t => t.Id == id);

        if (learningTask == null)
        {
            return ServiceResult<bool>.Fail("Document not found");
        }

        _traineeContext.LearningTasks.Remove(learningTask);

        await _traineeContext.SaveChangesAsync();

        return ServiceResult<bool>.Ok(true);
    }

    public async Task<ServiceResult<List<LearningTaskDto>>> GetAll()
    {
        var mentors = await _traineeContext.LearningTasks.ToListAsync();

        var learningTaskDtos = mentors
            .Select(t => _mapper.Map<LearningTaskDto>(t))
            .ToList();

        return ServiceResult<List<LearningTaskDto>>.Ok(learningTaskDtos);
    }

    public ServiceResult<LearningTaskDto> GetById(int id)
    {
        var mentorById = _traineeContext.LearningTasks.FirstOrDefault(t => t.Id == id);

        if (mentorById == null)
        {
            return ServiceResult<LearningTaskDto>.Fail("Document not found");
        }

        LearningTaskDto learningTask = _mapper.Map<LearningTaskDto>(mentorById);

        return ServiceResult<LearningTaskDto>.Ok(learningTask);
    }

    public async Task<ServiceResult<LearningTaskDto>> Update(int id, UpdateLearningTaskDto updatedDetails)
    {
        var learningTask = _traineeContext.LearningTasks.FirstOrDefault(t => t.Id == id);

        if (learningTask == null)
        {
            return ServiceResult<LearningTaskDto>.Fail("Document not found");
        }

        learningTask.Title = updatedDetails.Title!;
        learningTask.Description = updatedDetails.Description!;
        learningTask.ExpectedTechStack = updatedDetails.ExpectedTechStack!;
        learningTask.Status = updatedDetails.Status!;
        learningTask.DueDate = updatedDetails.DueDate!;
        learningTask.UpdatedDate = DateOnly.FromDateTime(DateTime.Now);

        await _traineeContext.SaveChangesAsync();

        var response = _mapper.Map<LearningTaskDto>(learningTask);

        return ServiceResult<LearningTaskDto>.Ok(response);
    }
}