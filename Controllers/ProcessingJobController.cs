using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrainineeAPI.Models;

namespace TrainineeAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class ProcessingJobController : ControllerBase
{
    private readonly ILogger<ProcessingJobController> _logger;
    private readonly IProcessJobService _processJobService;

    public ProcessingJobController(ILogger<ProcessingJobController> logger, IProcessJobService processJobService)
    {
        _logger = logger;
        _processJobService = processJobService;
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<ActionResult<ProcessingJob>> GetById(int id)
    {
        Console.WriteLine("Process job retry by id : " + id);
        var result = await _processJobService.GetById(id);

        if (result.Success)
        {
            return Ok(result);
        }
        else
        {
            return NotFound(result);
        }
    }

    [Authorize]
    [HttpPost("{id}/retry")]
    public async Task<ActionResult<ProcessingJob>> CreateJobRetry(int id)
    {
        Console.WriteLine("Create Process job retry by id : " + id);

        var result = await _processJobService.CreateJobRetry(id);

        if (result.Success)
        {
            return Ok(result);
        }
        else
        {
            return NotFound(result);
        }
    }
}
