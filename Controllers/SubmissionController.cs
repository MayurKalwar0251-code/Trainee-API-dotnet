using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrainineeAPI.DTOs;
using TrainineeAPI.Models;
using YamlDotNet.Core.Tokens;

namespace TrainineeAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class SubmissionController : ControllerBase
{
    private readonly ILogger<SubmissionController> _logger;
    private readonly ISubmissionService _submissionService;
    public SubmissionController(ILogger<SubmissionController> logger, ISubmissionService submissionService)
    {
        _logger = logger;
        _submissionService = submissionService;
    }

    [Authorize]
    [HttpGet("")]
    public async Task<ActionResult<IEnumerable<SubmissionDto>>> GetAllSubmission()
    {
        var submissions = await _submissionService.GetAll();
        return Ok(submissions);
    }

    [Authorize]
    [HttpGet("{id}")]
    public ActionResult<SubmissionDto> GetSubmissionById(int id)
    {
        ServiceResult<SubmissionDto> submissionById = _submissionService.GetById(id);

        if (submissionById.Data == null)
        {
            _logger.LogError(ErrorConstants.DocumentNotFound);
            return NotFound(submissionById);
        }

        return Ok(submissionById);
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<SubmissionDto>> CreateSubmission(CreateSubmissionDto submission)
    {
        Console.WriteLine("Submission Creation Started");
        ServiceResult<SubmissionDto> result = await _submissionService.Create(submission);
        if (result.Data == null)
        {
            _logger.LogError(ErrorConstants.DocumentNotFound);
            return NotFound(result);
        }
        _logger.LogInformation(MessagesConstants.CreatedSuccessfully);
        return Ok(result);
    }
}
