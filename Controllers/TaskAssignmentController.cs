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
        var taskAssignments = await _taskAssignmentService.GetAll();
        return Ok(taskAssignments);
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<ActionResult<TaskAssignmentDto>> GetTaskAssignmentById(int id)
    {
        ServiceResult<TaskAssignmentDto> taskAssignmentById = await _taskAssignmentService.GetById(id);

        if (taskAssignmentById.Data == null)
        {
            _logger.LogError(ErrorConstants.DocumentNotFound);
            return NotFound(taskAssignmentById);
        }

        return Ok(taskAssignmentById);
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<TaskAssignmentDto>> CreateTaskAssignment(CreateTaskAssignmentDto taskAssignment)
    {
        Console.WriteLine("Task Assignment Creation Started");
        ServiceResult<TaskAssignmentDto> result = await _taskAssignmentService.Create(taskAssignment);
        if (result.Data == null)
        {
            return NotFound(result);
        }
        _logger.LogInformation(MessagesConstants.CreatedSuccessfully);
        return Ok(result);
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<ActionResult<TaskAssignmentDto>> UpdateTaskAssignment(int id, UpdateTaskAssignmentDto updatedDetails)
    {
        ServiceResult<TaskAssignmentDto> updateTaskAssignment = await _taskAssignmentService.Update(id, updatedDetails);

        if (updateTaskAssignment.Data == null)
        {
            _logger.LogError(ErrorConstants.DocumentNotFound);
            return NotFound(updateTaskAssignment);
        }
        _logger.LogInformation(MessagesConstants.UpdatedSuccessfully);
        return Ok(updateTaskAssignment);
    }
}
