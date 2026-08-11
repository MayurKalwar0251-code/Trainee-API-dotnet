using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrainineeAPI.DTOs;
using YamlDotNet.Core.Tokens;

namespace TrainineeAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class TraineeController : ControllerBase
{

    private readonly ILogger<TraineeController> _logger;
    private readonly ITraineeService _traineeService;
    public TraineeController(ILogger<TraineeController> logger, ITraineeService traineeService)
    {
        _logger = logger;
        _traineeService = traineeService;
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("")]
    public async Task<ActionResult<IEnumerable<TraineeDto>>> GetAllTrainee([FromQuery] FilterTraineeDto filter)
    {
        if (UtilityFunctions.CheckHasFilterQuery(filter))
        {
            Console.WriteLine("WE are here in filter");
            var filterResult = await _traineeService.FilterByQuery(filter);
            return Ok(filterResult);
        }
        else
        {
            Console.WriteLine("WE are not here in filter");
            var traineeDtos = await _traineeService.GetAll();
            return Ok(traineeDtos);
        }
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<ActionResult<TraineeDto>> GetTraineeById(int id)
    {
        var traineeById = await _traineeService.GetById(id);

        if (!traineeById.Success)
        {
            _logger.LogError(ErrorConstants.DocumentNotFound);
            return NotFound(traineeById);
        }
        return Ok(traineeById);
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<TraineeDto>> CreateTrainee(CreateTraineeDto trainee)
    {
        Console.WriteLine("Trainee Creation Started");
        var result = await _traineeService.Create(trainee);
        _logger.LogInformation(MessagesConstants.CreatedSuccessfully);
        return Ok(result);
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<ActionResult<TraineeDto>> UpdateTrainee(int id, UpdateTraineeDto updatedDetails)
    {
        var updateTraine = await _traineeService.Update(id, updatedDetails);

        if (!updateTraine.Success)
        {
            _logger.LogError(ErrorConstants.DocumentNotFound);
            return NotFound(updateTraine);
        }
        _logger.LogInformation(MessagesConstants.UpdatedSuccessfully);
        return Ok(updateTraine);
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTrainee(int id)
    {
        var deleteStatus = await _traineeService.Delete(id);

        if (!deleteStatus.Success)
        {
            _logger.LogError(ErrorConstants.DocumentNotFound);
            return NotFound(deleteStatus);
        }

        _logger.LogInformation(MessagesConstants.DeletedSuccessfully);

        return Ok(new
        {
            StatusCode = StatusCodes.Status200OK,
            Message = MessagesConstants.DeletedSuccessfully
        });
    }

}
