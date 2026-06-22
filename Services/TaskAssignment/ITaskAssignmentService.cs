using TrainineeAPI.DTOs;
using TrainineeAPI.Models;

public interface ITaskAssignmentService
{
    Task<ServiceResult<List<TaskAssignmentDto>>> GetAll();

    Task<ServiceResult<TaskAssignmentDto>> GetById(int id);

    Task<ServiceResult<TaskAssignmentDto>> Create(CreateTaskAssignmentDto dto);

    Task<ServiceResult<TaskAssignmentDto>> Update(int id, UpdateTaskAssignmentDto dto);

}