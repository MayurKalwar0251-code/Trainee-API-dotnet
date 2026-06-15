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
        try
        {
            var submissions = await _submissionService.GetAll();
            return Ok(submissions);
        }
        catch (System.Exception)
        {
            _logger.LogError(ErrorConstants.InternalServerError);
            return Problem(ErrorConstants.InternalServerError);
        }
    }

    [Authorize]
    [HttpGet("{id}")]
    public ActionResult<SubmissionDto> GetSubmissionById(int id)
    {
        try
        {
            var submissionById = _submissionService.GetById(id);

            if (submissionById == null)
            {
                _logger.LogError(ErrorConstants.DocumentNotFound);
                return NotFound();
            }

            return Ok(submissionById);
        }
        catch (System.Exception)
        {
            _logger.LogError(ErrorConstants.InternalServerError);
            return Problem(ErrorConstants.InternalServerError);
        }
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<SubmissionDto>> CreateSubmission(CreateSubmissionDto submission)
    {
        try
        {
            Console.WriteLine("Submission Creation Started");
            ServiceResult<SubmissionDto> result = await _submissionService.Create(submission);
            if (result == null)
            {
                return Ok(new
                {
                    Message = "Task assignment id not found"
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
}
