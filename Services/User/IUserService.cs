using Microsoft.AspNetCore.Mvc;
using TrainineeAPI.DTOs;
public interface IUserService
{
    IActionResult SignUp();
    
    ServiceResult<Object> Login(LoginUserDto userReq);
}
