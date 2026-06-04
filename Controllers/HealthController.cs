using Microsoft.AspNetCore.Mvc;

namespace TrainineeAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class HealthController : ControllerBase
{

    private readonly ILogger<HealthController> _logger;

    public HealthController(ILogger<HealthController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetHealth")]
    public IActionResult GetHealth()
    {
        var response = new
        {
            status = "running",
            application = "Trainee Management API",
            timestamp = DateTime.Now,
        };
        return Ok(response);
    }

}
