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
    public TraineeController(ILogger<TraineeController> logger,ITraineeService traineeService)
    {
        _logger = logger;
        _traineeService = traineeService;
    }

    // [Authorize(Roles = "Admin")]
    [HttpGet("")]
    public async Task<ActionResult<IEnumerable<TraineeDto>>> GetAllTrainee([FromQuery] FilterTraineeDto filter)
    {
        try
        {
            if (UtilityFunctions.CheckHasFilterQuery(filter))
            {
                Console.WriteLine("WE are here in filter");
                var filterResult = await _traineeService.FilterByQuery(filter);
                return Ok(filterResult);
            }
            else
            {
                var traineeDtos = await _traineeService.GetAll();
                return Ok(traineeDtos);
            }
        }
        catch (System.Exception)
        {
            Console.WriteLine(ErrorConstants.InternalServerError);
            return Problem(ErrorConstants.InternalServerError);
        }
    }

    [Authorize]
    [HttpGet("{id}")]
    public ActionResult<TraineeDto> GetTraineeById(int id)
    {
        try
        {
            var traineeById = _traineeService.GetById(id);

            if (traineeById == null)
            {
                return NotFound();
            }

            return Ok(traineeById);
        }
        catch (System.Exception)
        {
            Console.WriteLine(ErrorConstants.InternalServerError);
            return Problem(ErrorConstants.InternalServerError);
        }
    }

    [Authorize]
    [HttpPost]
    public ActionResult<TraineeDto> CreateTrainee(CreateTraineeDto trainee)
    {
        try
        {
            return _traineeService.Create(trainee);
        }
        catch (System.Exception)
        {
            Console.WriteLine(ErrorConstants.InternalServerError);
            return Problem(ErrorConstants.InternalServerError);
        }
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<ActionResult<TraineeDto>> UpdateTrainee(int id, UpdateTraineeDto updatedDetails)
    {
        try
        {
            var updateTraine = await _traineeService.Update(id,updatedDetails);

            if (updateTraine == null)
            {
                return NotFound();
            }

            return Ok(updateTraine);
        }
        catch (System.Exception)
        {
            Console.WriteLine(ErrorConstants.InternalServerError);
            return Problem(ErrorConstants.InternalServerError);
        }
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTrainee(int id)
    {
        try
        {
            var deleteStatus = await _traineeService.Delete(id);

            if (!deleteStatus)
            {
                return NotFound();
            }

            return Ok(new
            {
                StatusCode = StatusCodes.Status200OK,
                Message = MessagesConstants.DeletedSuccessfully
            });
        }
        catch (System.Exception)
        {
            Console.WriteLine(ErrorConstants.InternalServerError);
            return Problem(ErrorConstants.InternalServerError);
        }
    }

}
