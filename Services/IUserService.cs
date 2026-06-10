using Microsoft.AspNetCore.Mvc;
using TrainineeAPI.DTOs;
public interface IUserService
{
    IActionResult SignUp();
    
    Object Login(LoginUserDto userReq);
}
