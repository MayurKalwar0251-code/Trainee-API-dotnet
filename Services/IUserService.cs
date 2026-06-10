using Microsoft.AspNetCore.Mvc;
public interface IUserService
{
    IActionResult SignUp();
    
    IActionResult Login();
}
