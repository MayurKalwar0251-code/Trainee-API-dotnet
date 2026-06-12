using TrainineeAPI.DTOs;
using TrainineeAPI.Models;

public interface ITaskAssignmentService
{
    Task<List<TaskAssignmentDto>> GetAll();

    TaskAssignmentDto? GetById(int id);

    Task<TaskAssignmentDto?> Create(CreateTaskAssignmentDto dto);

    Task<TaskAssignmentDto?> Update(int id, UpdateTaskAssignmentDto dto);

}