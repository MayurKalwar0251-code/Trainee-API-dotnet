using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using TrainineeAPI.DTOs;
using TrainineeAPI.Models;

public class TaskAssignmentService : ITaskAssignmentService
{
    private readonly TraineeContext _traineeContext;
    private readonly IMapper _mapper;
    private readonly ICacheService _cacheService;

    public TaskAssignmentService(TraineeContext traineeContext, IMapper mapper, ICacheService cacheService)
    {
        _traineeContext = traineeContext;
        _mapper = mapper;
        _cacheService = cacheService;
    }

    public async Task<ServiceResult<TaskAssignmentDto>> Create(CreateTaskAssignmentDto body)
    {
        // 1. Business Logic Validation (Fast fail without touching DB)
        if (body.DueDate < body.AssignedDate)
        {
            return ServiceResult<TaskAssignmentDto>.Fail(ErrorConstants.DueDateLesserError);
        }

        // 2. Validate Foreign Keys & Check Duplicates sequentially
        // Each line waits for the previous one to release the DbContext
        var traineeExists = await _traineeContext.Trainees.AnyAsync(i => i.Id == body.TraineeId);
        if (!traineeExists) return ServiceResult<TaskAssignmentDto>.Fail(ErrorConstants.DocumentNotFound);

        var taskExists = await _traineeContext.LearningTasks.AnyAsync(i => i.Id == body.LearningTaskId);
        if (!taskExists) return ServiceResult<TaskAssignmentDto>.Fail(ErrorConstants.DocumentNotFound);

        var mentorExists = await _traineeContext.Mentors.AnyAsync(i => i.Id == body.MentorId);
        if (!mentorExists) return ServiceResult<TaskAssignmentDto>.Fail(ErrorConstants.DocumentNotFound);

        var duplicateExists = await _traineeContext.TaskAssignments.AnyAsync(a =>
            a.TraineeId == body.TraineeId &&
            a.LearningTaskId == body.LearningTaskId &&
            a.MentorId == body.MentorId);

        if (duplicateExists)
        {
            return ServiceResult<TaskAssignmentDto>.Fail("This task assignment already exists.");
        }

        // 3. Map and prepare entity
        var id = _traineeContext.TaskAssignments.Count() == 0 ? 1 : _traineeContext.TaskAssignments.Max(t => t.Id) + 1;
        TaskAssignment taskAssignment = _mapper.Map<TaskAssignment>(body);
        taskAssignment.Id = id;
        var today = DateOnly.FromDateTime(DateTime.Now);
        taskAssignment.CreatedDate = today;
        taskAssignment.UpdatedDate = today;

        // 4. Save changes and return mapped DTO
        _traineeContext.TaskAssignments.Add(taskAssignment);
        await _traineeContext.SaveChangesAsync();

        TaskAssignmentDto taskAssignmentDto = _mapper.Map<TaskAssignmentDto>(taskAssignment);
        return ServiceResult<TaskAssignmentDto>.Ok(taskAssignmentDto);
    }


    async Task<ServiceResult<List<TaskAssignmentDto>>> ITaskAssignmentService.GetAll()
    {
        var taskAssignments = await _traineeContext.TaskAssignments.ToListAsync();

        var taskAssignmentDtos = taskAssignments
            .Select(t => _mapper.Map<TaskAssignmentDto>(t))
            .ToList();

        return ServiceResult<List<TaskAssignmentDto>>.Ok(taskAssignmentDtos);
    }

    async Task<ServiceResult<TaskAssignmentDto>> ITaskAssignmentService.GetById(int id)
    {
        string key = $"taskassignment:${id}";

        var data = await _cacheService.GetAsync<TaskAssignmentDto>(key);

        if (data != null)
        {
            Console.WriteLine("Fetched from Cache : " + key);
            return ServiceResult<TaskAssignmentDto>.Ok(data);
        }

        Console.WriteLine("Fetching from db : " + key);

        var taskAssignmentById = _traineeContext.TaskAssignments.FirstOrDefault(t => t.Id == id);

        if (taskAssignmentById == null)
        {
            return ServiceResult<TaskAssignmentDto>.Fail(ErrorConstants.DocumentNotFound);
        }

        TaskAssignmentDto taskAssignmentDto = _mapper.Map<TaskAssignmentDto>(taskAssignmentById);

        var cacheOptions = new DistributedCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(20));

        await _cacheService.SetAsync(key, taskAssignmentDto, cacheOptions);

        return ServiceResult<TaskAssignmentDto>.Ok(taskAssignmentDto);
    }

    async Task<ServiceResult<TaskAssignmentDto>> ITaskAssignmentService.Update(int id, UpdateTaskAssignmentDto updatedDetails)
    {
        TaskAssignment taskAssignment = _traineeContext.TaskAssignments.FirstOrDefault(t => t.Id == id)!;

        if (taskAssignment == null)
        {
            return ServiceResult<TaskAssignmentDto>.Fail(ErrorConstants.DocumentNotFound);
        }

        // validate traineeId,MentorId,LearningTaskId are in DB
        Trainee trainee = _traineeContext.Trainees.FirstOrDefault(i => i.Id == updatedDetails.TraineeId)!;
        LearningTask learningTask = _traineeContext.LearningTasks.FirstOrDefault(i => i.Id == updatedDetails.LearningTaskId)!;
        Mentor mentor = _traineeContext.Mentors.FirstOrDefault(i => i.Id == updatedDetails.MentorId)!;

        Console.WriteLine(trainee == null);
        Console.WriteLine(learningTask == null);
        Console.WriteLine(mentor == null);
        if (trainee == null || learningTask == null || mentor == null)
        {
            Console.WriteLine("Error in Validate ids");
            return ServiceResult<TaskAssignmentDto>.Fail(ErrorConstants.DocumentNotFound);
        }

        _mapper.Map(updatedDetails, taskAssignment);
        taskAssignment.UpdatedDate = DateOnly.FromDateTime(DateTime.Now);

        await _traineeContext.SaveChangesAsync();

        var response = _mapper.Map<TaskAssignmentDto>(taskAssignment);

        // invalidate cache data
        string key = $"taskassignment:${id}";
        await _cacheService.RemoveAsync(key);

        return ServiceResult<TaskAssignmentDto>.Ok(response);
    }
}