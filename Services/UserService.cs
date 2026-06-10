using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrainineeAPI.DTOs;
using TrainineeAPI.Models;
using BCrypt.Net;

class UserService : IUserService
{
    private readonly TraineeContext _traineeContext;
    private readonly IJWTService _jwtService;
    public UserService(TraineeContext traineeContext,IJWTService jWTService)
    {
        _traineeContext = traineeContext;
        _jwtService = jWTService;
    }
    object IUserService.Login(LoginUserDto userRequest)
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

        var jwt = _jwtService.GenerateToken(userRequest);

        return new {
            user,
            expiresIn = DateTime.Now.AddMinutes(15),
            token = jwt,
        };
    }

    IActionResult IUserService.SignUp()
    {
        throw new NotImplementedException();
    }

}