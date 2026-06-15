using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TrainineeAPI.DTOs;
using TrainineeAPI.Models;

public class MentorService : IMentorService
{
    private readonly TraineeContext _traineeContext;
    private readonly IMapper _mapper;

    public MentorService(TraineeContext traineeContext, IMapper mapper)
    {
        _traineeContext = traineeContext;
        _mapper = mapper;
    }

    public ServiceResult<MentorDto> Create(CreateMentorDto mentor)
    {
        var id = _traineeContext.Mentors.Count() == 0 ? 1 : _traineeContext.Mentors.Max(t => t.Id) + 1;

        Mentor newMentor = _mapper.Map<Mentor>(mentor);
        newMentor.Id = id;
        newMentor.CreatedDate = DateOnly.FromDateTime(DateTime.Now);
        newMentor.UpdatedDate = DateOnly.FromDateTime(DateTime.Now);

        MentorDto mentorDto = _mapper.Map<MentorDto>(newMentor);

        _traineeContext.Mentors.Add(newMentor);

        _traineeContext.SaveChangesAsync();

        return ServiceResult<MentorDto>.Ok(mentorDto);
    }

    public async Task<ServiceResult<bool>> Delete(int id)
    {
        var mentor = _traineeContext.Mentors.FirstOrDefault(t => t.Id == id);

        if (mentor == null)
        {
            return ServiceResult<bool>.Fail(ErrorConstants.DocumentNotFound);
        }

        _traineeContext.Mentors.Remove(mentor);

        await _traineeContext.SaveChangesAsync();

        return ServiceResult<bool>.Ok(true);
    }

    public async Task<ServiceResult<List<MentorDto>>> GetAll()
    {
        var mentors = await _traineeContext.Mentors.ToListAsync();

        var mentorDtos = mentors
            .Select(t => _mapper.Map<MentorDto>(t))
            .ToList();

        return ServiceResult<List<MentorDto>>.Ok(mentorDtos);
    }

    public ServiceResult<MentorDto> GetById(int id)
    {
        var mentorById = _traineeContext.Mentors.FirstOrDefault(t => t.Id == id);

        if (mentorById == null)
        {
            return ServiceResult<MentorDto>.Fail(ErrorConstants.DocumentNotFound);
        }

        MentorDto mentor = _mapper.Map<MentorDto>(mentorById);

        return ServiceResult<MentorDto>.Ok(mentor);
    }

    public async Task<ServiceResult<MentorDto>> Update(int id, UpdateMentorDto updatedDetails)
    {
        var mentor = _traineeContext.Mentors.FirstOrDefault(t => t.Id == id);

        if (mentor == null)
        {
            return ServiceResult<MentorDto>.Fail(ErrorConstants.DocumentNotFound);
        }

        _mapper.Map(updatedDetails, mentor);

        mentor.UpdatedDate = DateOnly.FromDateTime(DateTime.Now);

        await _traineeContext.SaveChangesAsync();

        var response = _mapper.Map<MentorDto>(mentor);

        return ServiceResult<MentorDto>.Ok(response);
    }
}