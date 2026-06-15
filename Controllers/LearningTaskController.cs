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
        var learningTasks = await _learningTaskService.GetAll();
        return Ok(learningTasks);
    }

    [Authorize]
    [HttpGet("{id}")]
    public ActionResult<LearningTaskDto> GetLearningTaskById(int id)
    {
        var learningTaskById = _learningTaskService.GetById(id);

        if (learningTaskById == null)
        {
            _logger.LogError(ErrorConstants.DocumentNotFound);
            return NotFound();
        }

        return Ok(learningTaskById);
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<LearningTaskDto>> CreateLearningTask(CreateLearningTaskDto learningTask)
    {
        Console.WriteLine("Learning Task Creation Started");
        var result = await _learningTaskService.Create(learningTask);
        _logger.LogInformation(MessagesConstants.CreatedSuccessfully);
        return Ok(result);
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<ActionResult<LearningTaskDto>> UpdateLearningTask(int id, UpdateLearningTaskDto updatedDetails)
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

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteLearningTask(int id)
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

}
