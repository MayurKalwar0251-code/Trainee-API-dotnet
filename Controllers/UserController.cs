using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using TrainineeAPI.DTOs;
namespace TrainineeAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;

    private readonly IUserService _userService;

    public UserController(ILogger<UserController> logger,IUserService userService)
    {
        _logger = logger;
        _userService = userService;
    }

    [HttpPost("/signup")]
    public IActionResult SignUp()
    {
        return Ok(new
        {
            message = MessagesConstants.SignUpSuccessfully
        });
    }
    
    [HttpPost("/login")]
    public IActionResult SignIn(LoginUserDto userBody)
    {
        var user = _userService.Login(userBody);

        if (user == null)
        {
            _logger.LogError(ErrorConstants.InvalidCredentials);
            return NotFound(new
            {
                ErrorConstants.InvalidCredentials
            });
        }

        _logger.LogInformation(MessagesConstants.LoginSuccessfully);

        return Ok(new
        {
            user,
            message = MessagesConstants.LoginSuccessfully
        });
    }
}
