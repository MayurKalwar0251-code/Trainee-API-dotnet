using TrainineeAPI.DTOs;
using TrainineeAPI.Models;

public interface ILearningTaskService
{
    Task<List<LearningTaskDto>> GetAll();

    LearningTaskDto? GetById(int id);

    LearningTaskDto Create(CreateLearningTaskDto dto);

    Task<LearningTaskDto?> Update(int id, UpdateLearningTaskDto dto);

    Task<bool> Delete(int id);
}
