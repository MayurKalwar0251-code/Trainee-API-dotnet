using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TrainineeAPI.DTOs;
using TrainineeAPI.Models;

// TODO user automapper

public class LearningTaskService : ILearningTaskService
{
    private readonly TraineeContext _traineeContext;
    private readonly IMapper _mapper;

    public LearningTaskService(TraineeContext traineeContext, IMapper mapper)
    {
        _traineeContext = traineeContext;
        _mapper = mapper;
    }

    public async Task<ServiceResult<LearningTaskDto>> Create(CreateLearningTaskDto learningTask)
    {
        var id = _traineeContext.LearningTasks.Count() == 0 ? 1 : _traineeContext.LearningTasks.Max(t => t.Id) + 1;

        LearningTask newLearningTask = _mapper.Map<LearningTask>(learningTask);
        newLearningTask.Id = id;
        newLearningTask.CreatedDate = DateOnly.FromDateTime(DateTime.Now);
        newLearningTask.UpdatedDate = DateOnly.FromDateTime(DateTime.Now);

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
            return ServiceResult<bool>.Fail(ErrorConstants.DocumentNotFound);
        }

        _traineeContext.LearningTasks.Remove(learningTask);

        await _traineeContext.SaveChangesAsync();

        return ServiceResult<bool>.Ok(true);
    }

    public async Task<ServiceResult<List<LearningTaskDto>>> GetAll()
    {
        var learningTasksWithCounts = await _traineeContext.LearningTasks.Select(lt => new
        {
            Task = lt,
            AssignedCount = _traineeContext.TaskAssignments.Count(a => a.LearningTaskId == lt.Id)
        })
        .ToListAsync();

        var learningTaskDtos = learningTasksWithCounts
            .Select(t =>
            {
                var dto = _mapper.Map<LearningTaskDto>(t.Task);
                dto.noOfAssignedTrainee = t.AssignedCount;
                return dto;
            })
            .ToList();
        
        return ServiceResult<List<LearningTaskDto>>.Ok(learningTaskDtos);
    }

    public ServiceResult<LearningTaskDto> GetById(int id)
    {
        var mentorById = _traineeContext.LearningTasks.FirstOrDefault(t => t.Id == id);

        if (mentorById == null)
        {
            return ServiceResult<LearningTaskDto>.Fail(ErrorConstants.DocumentNotFound);
        }

        LearningTaskDto learningTask = _mapper.Map<LearningTaskDto>(mentorById);

        return ServiceResult<LearningTaskDto>.Ok(learningTask);
    }

    public async Task<ServiceResult<LearningTaskDto>> Update(int id, UpdateLearningTaskDto updatedDetails)
    {
        var learningTask = _traineeContext.LearningTasks.FirstOrDefault(t => t.Id == id);

        if (learningTask == null)
        {
            return ServiceResult<LearningTaskDto>.Fail(ErrorConstants.DocumentNotFound);
        }

        _mapper.Map(updatedDetails,learningTask);
        learningTask.UpdatedDate = DateOnly.FromDateTime(DateTime.Now);

        await _traineeContext.SaveChangesAsync();

        var response = _mapper.Map<LearningTaskDto>(learningTask);

        return ServiceResult<LearningTaskDto>.Ok(response);
    }
}