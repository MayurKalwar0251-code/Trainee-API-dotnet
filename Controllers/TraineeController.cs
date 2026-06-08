using Microsoft.AspNetCore.Mvc;
using TrainineeAPI.Models;
using TrainineeAPI.DTOs;

namespace TrainineeAPI.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class TraineeController : ControllerBase
{

    private readonly ILogger<TraineeController> _logger;
    private readonly ITraineeService _traineeService;
    public TraineeController(ILogger<TraineeController> logger,ITraineeService traineeService)
    {
        _logger = logger;
        _traineeService = traineeService;
    }

    [HttpGet("/all")]
    public async Task<ActionResult<IEnumerable<TraineeDto>>> GetAllTrainee()
    {
        var traineeDtos = await _traineeService.GetAll();

        return Ok(traineeDtos);
    }

    [HttpGet("")]
    public async Task<ActionResult<IEnumerable<TraineeDto>>> FilterSearchTrainee(string search)
    {
        var filterResult = await _traineeService.FilterBySearch(search);

        return Ok(filterResult);
    }

    [HttpGet("{id}")]
    public ActionResult<TraineeDto> GetTraineeById(int id)
    {

        var traineeById = _traineeService.GetById(id);

        if (traineeById == null)
        {
            return NotFound();
        }

        return Ok(traineeById);
    }

    [HttpPost]
    public ActionResult<TraineeDto> CreateTrainee(CreateTraineeDto trainee)
    {
        return _traineeService.Create(trainee);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<TraineeDto>> UpdateTrainee(int id, UpdateTraineeDto updatedDetails)
    {
        var updateTraine = await _traineeService.Update(id,updatedDetails);

        if (updateTraine == null)
        {
            return NotFound();
        }

        return Ok(updateTraine);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTrainee(int id)
    {
        var deleteStatus = await _traineeService.Delete(id);

        if (!deleteStatus)
        {
            return NotFound();
        }

        return Ok(new
        {
            StatusCode = 200,
            Message = "Deleted SUccessfully"
        });
    }

}
