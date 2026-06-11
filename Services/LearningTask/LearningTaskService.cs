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

    public LearningTaskDto Create(CreateLearningTaskDto learningTask)
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

        LearningTaskDto mentorDto = _mapper.Map<LearningTaskDto>(newLearningTask);

        _traineeContext.LearningTasks.Add(newLearningTask);

        _traineeContext.SaveChangesAsync();

        return mentorDto;
    }

    public async Task<bool> Delete(int id)
    {
        var learningTask = _traineeContext.LearningTasks.FirstOrDefault(t => t.Id == id);

        if (learningTask == null)
        {
            return false;
        }

        _traineeContext.LearningTasks.Remove(learningTask);

        await _traineeContext.SaveChangesAsync();

        return true;
    }

    public async Task<List<LearningTaskDto>> GetAll()
    {
        var mentors = await _traineeContext.LearningTasks.ToListAsync();

        var mentorDtos = mentors
            .Select(t => _mapper.Map<LearningTaskDto>(t))
            .ToList();

        return mentorDtos;
    }

    public LearningTaskDto? GetById(int id)
    {
        var mentorById = _traineeContext.LearningTasks.FirstOrDefault(t => t.Id == id);

        if (mentorById == null)
        {
            return null;
        }

        LearningTaskDto learningTask = _mapper.Map<LearningTaskDto>(mentorById);

        return learningTask;
    }

    public async Task<LearningTaskDto?> Update(int id, UpdateLearningTaskDto updatedDetails)
    {
        var learningTask = _traineeContext.LearningTasks.FirstOrDefault(t => t.Id == id);

        if (learningTask == null)
        {
            return null;
        }

        learningTask.Title = updatedDetails.Title!;
        learningTask.Description = updatedDetails.Description!;
        learningTask.ExpectedTechStack = updatedDetails.ExpectedTechStack!;
        learningTask.Status = updatedDetails.Status!;
        learningTask.DueDate = updatedDetails.DueDate!;
        learningTask.UpdatedDate = DateOnly.FromDateTime(DateTime.Now);

        await _traineeContext.SaveChangesAsync();

        var response = _mapper.Map<LearningTaskDto>(learningTask);

        return response;
    }
}