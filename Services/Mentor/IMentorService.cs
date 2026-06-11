using TrainineeAPI.DTOs;
using TrainineeAPI.Models;

public interface IMentorService
{
    Task<List<MentorDto>> GetAll();

    MentorDto? GetById(int id);

    MentorDto Create(CreateMentorDto dto);

    Task<MentorDto?> Update(int id, UpdateMentorDto dto);

    Task<bool> Delete(int id);
}
