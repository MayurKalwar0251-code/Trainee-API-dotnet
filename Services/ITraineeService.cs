using TrainineeAPI.DTOs;

public interface ITraineeService
{
    Task<List<TraineeDto>> GetAll();

    TraineeDto? GetById(int id);

    TraineeDto Create(CreateTraineeDto dto);

    Task<TraineeDto?> Update(int id, UpdateTraineeDto dto);

    Task<bool> Delete(int id);
    
}
