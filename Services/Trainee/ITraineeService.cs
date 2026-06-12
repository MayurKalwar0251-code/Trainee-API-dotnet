using TrainineeAPI.DTOs;

public interface ITraineeService
{
    Task<ServiceResult<List<TraineeDto>>> GetAll();

    ServiceResult<TraineeDto> GetById(int id);

    ServiceResult<TraineeDto> Create(CreateTraineeDto dto);

    Task<ServiceResult<TraineeDto>> Update(int id, UpdateTraineeDto dto);

    Task<ServiceResult<bool>> Delete(int id);

    Task<ServiceResult<List<TraineeDto>>> FilterBySearch(string search); 
    Task<ServiceResult<List<TraineeDto>>> FilterByQuery(FilterTraineeDto filter); 
    
}
