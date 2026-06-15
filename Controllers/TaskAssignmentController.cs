using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrainineeAPI.DTOs;
using TrainineeAPI.Models;
using YamlDotNet.Core.Tokens;

namespace TrainineeAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class TaskAssignmentController : ControllerBase
{
    private readonly ILogger<TaskAssignmentController> _logger;
    private readonly ITaskAssignmentService _taskAssignmentService;
    public TaskAssignmentController(ILogger<TaskAssignmentController> logger, ITaskAssignmentService taskAssignmentService)
    {
        _logger = logger;
        _taskAssignmentService = taskAssignmentService;
    }

    [Authorize]
    [HttpGet("")]
    public async Task<ActionResult<IEnumerable<TaskAssignmentDto>>> GetAllTaskAssignment()
    {
        try
        {
            var taskAssignments = await _taskAssignmentService.GetAll();
            return Ok(taskAssignments);
        }
        catch (System.Exception)
        {
            _logger.LogError(ErrorConstants.InternalServerError);
            return Problem(ErrorConstants.InternalServerError);
        }
    }

    [Authorize]
    [HttpGet("{id}")]
    public ActionResult<TaskAssignmentDto> GetTaskAssignmentById(int id)
    {
        try
        {
            var taskAssignmentById = _taskAssignmentService.GetById(id);

            if (taskAssignmentById == null)
            {
                _logger.LogError(ErrorConstants.DocumentNotFound);
                return NotFound();
            }

            return Ok(taskAssignmentById);
        }
        catch (System.Exception)
        {
            _logger.LogError(ErrorConstants.InternalServerError);
            return Problem(ErrorConstants.InternalServerError);
        }
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<TaskAssignmentDto>> CreateTaskAssignment(CreateTaskAssignmentDto taskAssignment)
    {
        try
        {
            Console.WriteLine("Task Assignment Creation Started");
            ServiceResult<TaskAssignmentDto> result = await _taskAssignmentService.Create(taskAssignment);
            if (result == null)
            {
                return Ok(new
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = ErrorConstants.InternalServerError
                });
            }
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
    public async Task<ActionResult<TaskAssignmentDto>> UpdateTaskAssignment(int id, UpdateTaskAssignmentDto updatedDetails)
    {
        try
        {
            ServiceResult<TaskAssignmentDto> updateTaskAssignment = await _taskAssignmentService.Update(id, updatedDetails);

            if (updateTaskAssignment == null)
            {
                _logger.LogError(ErrorConstants.DocumentNotFound);
                return NotFound();
            }
            _logger.LogInformation(MessagesConstants.UpdatedSuccessfully);
            return Ok(updateTaskAssignment);
        }
        catch (System.Exception)
        {
            _logger.LogError(ErrorConstants.InternalServerError);
            return Problem(ErrorConstants.InternalServerError);
        }
    }
}
