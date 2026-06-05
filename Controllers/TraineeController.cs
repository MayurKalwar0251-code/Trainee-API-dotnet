using Microsoft.AspNetCore.Mvc;
using TrainineeAPI.Models;
using TrainineeAPI.DTOs;
using YamlDotNet.Core.Tokens;

namespace TrainineeAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class TraineeController : ControllerBase
{

    private static List<Trainee> Trainees {get; set;} = new List<Trainee> {};

    private readonly ILogger<TraineeController> _logger;
    private readonly ITraineeService _traineeService;
    public TraineeController(ILogger<TraineeController> logger,ITraineeService traineeService)
    {
        _logger = logger;
        _traineeService = traineeService;
    }

    [HttpGet]
    public ActionResult<IEnumerable<TraineeDto>> GetAllTrainee()
    {
        var traineeDtos = _traineeService.GetAll();

        return Ok(traineeDtos);
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
    public ActionResult<TraineeDto> UpdateTrainee(int id, UpdateTraineeDto updatedDetails)
    {
        var updateTraine = _traineeService.Update(id,updatedDetails);

        if (updateTraine == null)
        {
            return NotFound();
        }

        return Ok(updateTraine);
    }


    [HttpPatch("{id}")]
    public ActionResult<TraineeDto> UpdateTraineeUsingPatch(int id, UpdateTraineeDto updatedDetails)
    {
        var updateTraine = _traineeService.UpdateUsingPatch(id,updatedDetails);

        if (updateTraine == null)
        {
            return NotFound();
        }

        return Ok(updateTraine);
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteTrainee(int id)
    {
        var deleteStatus = _traineeService.Delete(id);

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
