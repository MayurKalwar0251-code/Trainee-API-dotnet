using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using TrainineeAPI.DTOs;
using TrainineeAPI.Models;

public class MentorService : IMentorService
{
    private readonly TraineeContext _traineeContext;
    private readonly IMapper _mapper;
    private readonly ICacheService _cacheService;

    public MentorService(TraineeContext traineeContext, IMapper mapper, ICacheService cacheService)
    {
        _traineeContext = traineeContext;
        _mapper = mapper;
        _cacheService = cacheService;
    }

    public async Task<ServiceResult<MentorDto>> Create(CreateMentorDto mentor)
    {
        // check email already exists
        var existsUser = await _traineeContext.Users.FirstOrDefaultAsync(x => x.Email == mentor.Email);

        if (existsUser != null)
        {
            return ServiceResult<MentorDto>.Fail("User Already Exists");
        } 

        var user = new User
        {
            Id = _traineeContext.Users.Count() == 0 ? 1 : _traineeContext.Users.Max(t => t.Id) + 1,
            Username = mentor.FirstName!,
            Email = mentor.Email!,
            PasswordHash = PasswordUtility.HashUserPassword(mentor.Password),
            Role = "Mentor",
            CreatedDate = DateOnly.FromDateTime(DateTime.Now),
            UpdatedDate = DateOnly.FromDateTime(DateTime.Now)
        };

        Console.WriteLine(JsonSerializer.Serialize(user));

        await _traineeContext.Users.AddAsync(user);

        await _traineeContext.SaveChangesAsync(); 

        var id = _traineeContext.Mentors.Count() == 0 ? 1 : _traineeContext.Mentors.Max(t => t.Id) + 1;

        Mentor newMentor = new Mentor
        {
            Id = id,
            UserId = user.Id,
            FirstName = mentor.FirstName,
            LastName = mentor.LastName,
            Email = mentor.Email,
            Expertise = mentor.Expertise,
            Status = "Active",
            CreatedDate = DateOnly.FromDateTime(DateTime.Now),
            UpdatedDate = DateOnly.FromDateTime(DateTime.Now),
        };

        Console.WriteLine(JsonSerializer.Serialize(newMentor));

        _traineeContext.Mentors.Add(newMentor);

        await _traineeContext.SaveChangesAsync();

        MentorDto mentorDto = _mapper.Map<MentorDto>(newMentor);

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

        // invalidate cache data
        string key = $"mentor:{id}";
        await _cacheService.RemoveAsync(key);

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

    public async Task<ServiceResult<MentorDto>> GetById(int id)
    {
        string key = $"mentor:{id}";
        var data = await _cacheService.GetAsync<MentorDto>(key);

        if (data != null)
        {
            Console.WriteLine("Fetched from Cache : " + key);
            return ServiceResult<MentorDto>.Ok(data);   
        }

        Console.WriteLine("Fetching from db : " + key);
        var mentorById = _traineeContext.Mentors.FirstOrDefault(t => t.Id == id);

        if (mentorById == null)
        {
            return ServiceResult<MentorDto>.Fail(ErrorConstants.DocumentNotFound);
        }

        MentorDto mentor = _mapper.Map<MentorDto>(mentorById);

        var cacheOptions = new DistributedCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(20));

        await _cacheService.SetAsync(key,mentor,cacheOptions);

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

        // invalidate cache data
        string key = $"mentor:{id}";
        await _cacheService.RemoveAsync(key);

        return ServiceResult<MentorDto>.Ok(response);
    }
}