using Microsoft.AspNetCore.Mvc;

namespace TrainineeAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;

    public UserController(ILogger<UserController> logger)
    {
        _logger = logger;
    }

    [HttpPost("/signup")]
    public IActionResult SignUp()
    {
        return Ok(new
        {
            message = "Sign up"
        });
    }
    
    [HttpPost("/login")]
    public IActionResult SignIn()
    {
        return Ok(new
        {
            message = "Sign In"
        });
    }
}
