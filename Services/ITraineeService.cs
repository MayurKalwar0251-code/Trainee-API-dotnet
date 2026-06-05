using TrainineeAPI.DTOs;

public interface ITraineeService
{
    List<TraineeDto> GetAll();

    TraineeDto? GetById(int id);

    TraineeDto Create(CreateTraineeDto dto);

    TraineeDto? Update(int id, UpdateTraineeDto dto);

    
    TraineeDto? UpdateUsingPatch(int id, UpdateTraineeDto dto);

    bool Delete(int id);
    
}
