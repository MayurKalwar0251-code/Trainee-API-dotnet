using TrainineeAPI.DTOs;
using TrainineeAPI.Models;

public interface ILearningTaskService
{
    Task<ServiceResult<List<LearningTaskDto>>> GetAll();

    ServiceResult<LearningTaskDto> GetById(int id);

    Task<ServiceResult<LearningTaskDto>> Create(CreateLearningTaskDto dto);

    Task<ServiceResult<LearningTaskDto>> Update(int id, UpdateLearningTaskDto dto);

    Task<ServiceResult<bool>> Delete(int id);
}
