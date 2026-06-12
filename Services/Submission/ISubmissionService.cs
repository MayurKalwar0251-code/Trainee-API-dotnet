using TrainineeAPI.DTOs;
using TrainineeAPI.Models;

public interface ISubmissionService
{
    Task<List<SubmissionDto>> GetAll();

    SubmissionDto? GetById(int id);

    Task<SubmissionDto> Create(CreateSubmissionDto dto);
}
