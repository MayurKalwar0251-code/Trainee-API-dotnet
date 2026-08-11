using TrainineeAPI.DTOs;

public interface ITraineeService
{
    Task<ServiceResult<List<TraineeDto>>> GetAll();

    Task<ServiceResult<TraineeDto>> GetById(int id);

    Task<ServiceResult<TraineeDto>> Create(CreateTraineeDto dto);

    Task<ServiceResult<TraineeDto>> Update(int id, UpdateTraineeDto dto);

    Task<ServiceResult<bool>> Delete(int id);

    Task<ServiceResult<List<TraineeDto>>> FilterBySearch(string search); 
    Task<ServiceResult<PagedResponse<TraineeDto>>> FilterByQuery(FilterTraineeDto filter); 
    
}
