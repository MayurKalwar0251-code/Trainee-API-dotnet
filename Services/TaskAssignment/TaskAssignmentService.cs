using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using TrainineeAPI.DTOs;
using TrainineeAPI.Models;

public class TaskAssignmentService : ITaskAssignmentService
{
    private readonly TraineeContext _traineeContext;
    private readonly IMapper _mapper;

    public TaskAssignmentService(TraineeContext traineeContext, IMapper mapper)
    {
        _traineeContext = traineeContext;
        _mapper = mapper;
    }

    async Task<ServiceResult<TaskAssignmentDto>> ITaskAssignmentService.Create(CreateTaskAssignmentDto body)
    {
        // validate traineeId,MentorId,LearningTaskId are in DB
        Trainee trainee = _traineeContext.Trainees.FirstOrDefault(i => i.Id == body.TraineeId)!;
        LearningTask learningTask = _traineeContext.LearningTasks.FirstOrDefault(i => i.Id == body.LearningTaskId)!;
        Mentor mentor = _traineeContext.Mentors.FirstOrDefault(i => i.Id == body.MentorId)!;

        Console.WriteLine(trainee == null);
        Console.WriteLine(learningTask == null);
        Console.WriteLine(mentor == null);
        if (trainee == null || learningTask == null || mentor == null)
        {
            Console.WriteLine("Error in Validate ids");
            return ServiceResult<TaskAssignmentDto>.Fail(ErrorConstants.DocumentNotFound);
        }

        // compare due date and assignment date
        if (body.DueDate < body.AssignedDate)
        {
            Console.WriteLine("Error in Due Date <");
            return ServiceResult<TaskAssignmentDto>.Fail("Due Date Should be greater than Assigned Date");
        }

        var id = _traineeContext.TaskAssignments.Count() == 0 ? 1 : _traineeContext.TaskAssignments.Max(t => t.Id) + 1;

        TaskAssignment taskAssignment = _mapper.Map<TaskAssignment>(body);
        taskAssignment.Id = id;
        taskAssignment.CreatedDate = DateOnly.FromDateTime(DateTime.Now);
        taskAssignment.UpdatedDate = DateOnly.FromDateTime(DateTime.Now);

        TaskAssignmentDto taskAssignmentDto = _mapper.Map<TaskAssignmentDto>(taskAssignment);
        _traineeContext.TaskAssignments.Add(taskAssignment);
        await _traineeContext.SaveChangesAsync();

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

    ServiceResult<TaskAssignmentDto> ITaskAssignmentService.GetById(int id)
    {
        var taskAssignmentById = _traineeContext.TaskAssignments.FirstOrDefault(t => t.Id == id);

        if (taskAssignmentById == null)
        {
            return ServiceResult<TaskAssignmentDto>.Fail(ErrorConstants.DocumentNotFound);
        }

        TaskAssignmentDto taskAssignmentDto = _mapper.Map<TaskAssignmentDto>(taskAssignmentById);

        return ServiceResult<TaskAssignmentDto>.Ok(taskAssignmentDto);
    }

    async Task<ServiceResult<TaskAssignmentDto>> ITaskAssignmentService.Update(int id, UpdateTaskAssignmentDto updatedDetails)
    {
        TaskAssignment taskAssignment = _traineeContext.TaskAssignments.FirstOrDefault(t => t.Id == id)!;

        if (taskAssignment == null)
        {
            return ServiceResult<TaskAssignmentDto>.Fail(ErrorConstants.DocumentNotFound);
        }

        _mapper.Map(updatedDetails,taskAssignment);
        taskAssignment.UpdatedDate = DateOnly.FromDateTime(DateTime.Now);

        await _traineeContext.SaveChangesAsync();

        var response = _mapper.Map<TaskAssignmentDto>(taskAssignment);

        return ServiceResult<TaskAssignmentDto>.Ok(response);
    }
}