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

    private readonly IJWTService _jwtService;

    private readonly IUserService _userService;

    private readonly IConfiguration _configuration;

    public UserController(ILogger<UserController> logger,IUserService userService, IConfiguration configuration, IJWTService jWTService)
    {
        _logger = logger;
        _configuration = configuration;
        _jwtService = jWTService;
        _userService = userService;
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
    public IActionResult SignIn(LoginUserDto userBody)
    {
        var user = _userService.Login(userBody);

        if (user == null)
        {
            return NotFound("Invalid Credentials or User Not Found");
        }

        return Ok(user);
    }
}
