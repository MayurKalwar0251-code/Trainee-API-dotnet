using TrainineeAPI.DTOs;
using TrainineeAPI.Models;

public interface ISubmissionService
{
    Task<ServiceResult<List<SubmissionDto>>> GetAll();

    ServiceResult<SubmissionDto> GetById(int id);

    Task<ServiceResult<SubmissionDto>> Create(CreateSubmissionDto dto);

    Task<ServiceResult<IEnumerable<SubmissionFile>>> SubmitSubmissionFile(int id,SubmitSubmissionFileDto submit);
}
