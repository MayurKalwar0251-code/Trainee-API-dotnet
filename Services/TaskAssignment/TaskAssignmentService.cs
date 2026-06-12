using System.Threading.Tasks;
using AutoMapper;
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

    async Task<TaskAssignmentDto?> ITaskAssignmentService.Create(CreateTaskAssignmentDto body)
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
            return null;
        }

        // compare due date and assignment date
        if (body.DueDate < body.AssignedDate)
        {
            Console.WriteLine("Error in Due Date <");
            return null;
        }

        var id = _traineeContext.TaskAssignments.Count() == 0 ? 1 : _traineeContext.TaskAssignments.Max(t => t.Id) + 1;

        TaskAssignment taskAssignment = new TaskAssignment
        {
            Id = id,
            TraineeId = body.TraineeId,
            MentorId = body.TraineeId,
            LearningTaskId = body.LearningTaskId,
            Status = body.Status,
            Remarks = body.Remarks,
            DueDate = body.DueDate,
            AssignedDate = body.AssignedDate,
            CreatedDate = DateOnly.FromDateTime(DateTime.Now),
            UpdatedDate = DateOnly.FromDateTime(DateTime.Now),
        };

        TaskAssignmentDto taskAssignmentDto = _mapper.Map<TaskAssignmentDto>(taskAssignment);
        _traineeContext.TaskAssignments.Add(taskAssignment);
        await _traineeContext.SaveChangesAsync();

        return taskAssignmentDto;
    }

    async Task<List<TaskAssignmentDto>> ITaskAssignmentService.GetAll()
    {
        var taskAssignments = await _traineeContext.TaskAssignments.ToListAsync();

        var taskAssignmentDtos = taskAssignments
            .Select(t => _mapper.Map<TaskAssignmentDto>(t))
            .ToList();

        return taskAssignmentDtos;
    }

    TaskAssignmentDto? ITaskAssignmentService.GetById(int id)
    {
        var taskAssignmentById = _traineeContext.TaskAssignments.FirstOrDefault(t => t.Id == id);

        if (taskAssignmentById == null)
        {
            return null;
        }

        TaskAssignmentDto taskAssignmentDto = _mapper.Map<TaskAssignmentDto>(taskAssignmentById);

        return taskAssignmentDto;
    }

    async Task<TaskAssignmentDto?> ITaskAssignmentService.Update(int id, UpdateTaskAssignmentDto updatedDetails)
    {
        TaskAssignment taskAssignment = _traineeContext.TaskAssignments.FirstOrDefault(t => t.Id == id)!;

        if (taskAssignment == null)
        {
            return null;
        }

        taskAssignment.TraineeId = updatedDetails.TraineeId;
        taskAssignment.MentorId = updatedDetails.MentorId;
        taskAssignment.LearningTaskId = updatedDetails.LearningTaskId;
        taskAssignment.Status = updatedDetails.Status!;
        taskAssignment.Remarks = updatedDetails.Remarks!;
        taskAssignment.DueDate = updatedDetails.DueDate!;
        taskAssignment.AssignedDate = updatedDetails.AssignedDate!;
        taskAssignment.UpdatedDate = DateOnly.FromDateTime(DateTime.Now);

        await _traineeContext.SaveChangesAsync();

        var response = _mapper.Map<TaskAssignmentDto>(taskAssignment);

        return response;
    }
}