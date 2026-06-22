using TrainineeAPI.DTOs;
using TrainineeAPI.Models;

public interface IMentorService
{
    Task<ServiceResult<List<MentorDto>>> GetAll();

    Task<ServiceResult<MentorDto>> GetById(int id);

    ServiceResult<MentorDto> Create(CreateMentorDto dto);

    Task<ServiceResult<MentorDto>> Update(int id, UpdateMentorDto dto);

    Task<ServiceResult<bool>> Delete(int id);
}
