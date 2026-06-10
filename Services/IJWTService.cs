using Microsoft.AspNetCore.Mvc;
using TrainineeAPI.DTOs;
public interface IJWTService
{
    string GenerateToken(LoginUserDto loginUserDto);
}
