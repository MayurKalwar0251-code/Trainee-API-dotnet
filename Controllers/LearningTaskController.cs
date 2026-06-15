using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrainineeAPI.DTOs;
using TrainineeAPI.Models;
using YamlDotNet.Core.Tokens;

namespace TrainineeAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class LearningTaskController : ControllerBase
{
    private readonly ILogger<LearningTaskController> _logger;
    private readonly ILearningTaskService _learningTaskService;
    public LearningTaskController(ILogger<LearningTaskController> logger, ILearningTaskService learningTaskService)
    {
        _logger = logger;
        _learningTaskService = learningTaskService;
    }

    [Authorize]
    [HttpGet("")]
    public async Task<ActionResult<IEnumerable<LearningTaskDto>>> GetAllLearningTask()
    {
        try
        {
            var learningTasks = await _learningTaskService.GetAll();
            return Ok(learningTasks);
        }
        catch (System.Exception)
        {
            _logger.LogError(ErrorConstants.InternalServerError);
            return Problem(ErrorConstants.InternalServerError);
        }
    }

    [Authorize]
    [HttpGet("{id}")]
    public ActionResult<LearningTaskDto> GetLearningTaskById(int id)
    {
        try
        {
            var learningTaskById = _learningTaskService.GetById(id);

            if (learningTaskById == null)
            {
                _logger.LogError(ErrorConstants.DocumentNotFound);
                return NotFound();
            }

            return Ok(learningTaskById);
        }
        catch (System.Exception)
        {
            _logger.LogError(ErrorConstants.InternalServerError);
            return Problem(ErrorConstants.InternalServerError);
        }
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<LearningTaskDto>> CreateLearningTask(CreateLearningTaskDto learningTask)
    {
        try
        {
            Console.WriteLine("Learning Task Creation Started");
            var result = await _learningTaskService.Create(learningTask);
            _logger.LogInformation(MessagesConstants.CreatedSuccessfully);
            return Ok(result);
        }
        catch (System.Exception)
        {
            _logger.LogError(ErrorConstants.InternalServerError);
            return Problem(ErrorConstants.InternalServerError);
        }
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<ActionResult<LearningTaskDto>> UpdateLearningTask(int id, UpdateLearningTaskDto updatedDetails)
    {
        try
        {
            var updateLearningTask = await _learningTaskService.Update(id, updatedDetails);

            if (updateLearningTask == null)
            {
                _logger.LogError(ErrorConstants.DocumentNotFound);
                return NotFound();
            }
            _logger.LogInformation(MessagesConstants.UpdatedSuccessfully);
            return Ok(updateLearningTask);
        }
        catch (System.Exception)
        {
            _logger.LogError(ErrorConstants.InternalServerError);
            return Problem(ErrorConstants.InternalServerError);
        }
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteLearningTask(int id)
    {
        try
        {
            var deleteStatus = await _learningTaskService.Delete(id);

            if (!deleteStatus.Data)
            {
                _logger.LogError(ErrorConstants.DocumentNotFound);
                return NotFound();
            }

            _logger.LogInformation(MessagesConstants.DeletedSuccessfully);

            return Ok(new
            {
                StatusCode = StatusCodes.Status200OK,
                Message = MessagesConstants.DeletedSuccessfully
            });
        }
        catch (System.Exception)
        {
            _logger.LogError(ErrorConstants.InternalServerError);
            return Problem(ErrorConstants.InternalServerError);
        }
    }

}
