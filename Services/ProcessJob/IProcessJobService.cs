using TrainineeAPI.DTOs;
using TrainineeAPI.Models;

public interface IProcessJobService
{
    Task<ServiceResult<ProcessingJob>> GetById(int id);

    Task<ServiceResult<ProcessingJob>> CreateJobRetry(int id);
}
