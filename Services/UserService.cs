using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrainineeAPI.DTOs;
using TrainineeAPI.Models;
using BCrypt.Net;
using AutoMapper;

class UserService : IUserService
{
    private readonly TraineeContext _traineeContext;
    private readonly IJWTService _jwtService;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;
    public UserService(TraineeContext traineeContext,IJWTService jWTService, IMapper mapper, IConfiguration configuration)
    {
        _traineeContext = traineeContext;
        _jwtService = jWTService;
        _mapper = mapper;
        _configuration = configuration;
    }
    object? IUserService.Login(LoginUserDto userRequest)
    {
        var user = _traineeContext.Users.FirstOrDefault(u => u.Email == userRequest.Email);

        if (user == null)
        {
            // User doesnt exist
            return null;    
        }

        // create hash for pass
        string hashedPass = PasswordUtility.HashUserPassword(userRequest.PasswordHash);
        // compare passwords
        bool checkPass = PasswordUtility.VerifyUserPassword(userRequest.PasswordHash,user.PasswordHash);
        if (!checkPass)
        {
            return null;
        }

        var jwt = _jwtService.GenerateToken(user);

        UserResponseDto userDto = _mapper.Map<UserResponseDto>(user);

        return new {
            userDto,
            expiresIn = DateTime.Now.AddMinutes(double.Parse(_configuration["Jwt:Expiry"]!)),
            token = jwt,
        };
    }

    IActionResult IUserService.SignUp()
    {
        throw new NotImplementedException();
    }

}