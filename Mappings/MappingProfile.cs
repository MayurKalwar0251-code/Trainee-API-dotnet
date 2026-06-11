using AutoMapper;
using TrainineeAPI.DTOs;
using TrainineeAPI.Models;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Maps properties with identical names automatically
        CreateMap<User, UserResponseDto>().ReverseMap();
        CreateMap<Trainee, TraineeDto>().ReverseMap();
    }
}
