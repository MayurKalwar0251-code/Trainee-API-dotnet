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
    private readonly ILocalFileStorage _localFileStorage;
    public SubmissionController(ILogger<SubmissionController> logger, ISubmissionService submissionService, ILocalFileStorage localFileStorage)
    {
        _logger = logger;
        _submissionService = submissionService;
        _localFileStorage = localFileStorage;
    }


    [Authorize]
    [HttpGet("task/{id}")]
    public async Task<ActionResult<IEnumerable<SubmissionDto>>> GetSubmissionOfTask(int id)
    {
        var submissions = await _submissionService.GetSubmissionOfTask(id);
        return Ok(submissions);
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
    public async Task<ActionResult<SubmissionDto>> GetSubmissionById(int id)
    {
        ServiceResult<SubmissionDto> submissionById = await _submissionService.GetById(id);

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

    [Authorize]
    [HttpPost("{id}/files")]
    public async Task<IActionResult> PostSubmissionFile(int id, SubmitSubmissionFileDto submit)
    {
        Console.WriteLine("Post api controller");
        foreach (var file in submit.Files)
        {
            var validateFile = ValidateFile.FileValidator(file);
            if (!validateFile.isValid)
            {
                return Problem(statusCode: 413, detail: validateFile.ErrorMessage);
            }
        }
        var result = await _submissionService.SubmitSubmissionFile(id, submit);
        return Ok(result);
    }

    [Authorize]
    [HttpGet("{id}/download")]
    public async Task<IActionResult> GetSubmissionFile(int id)
    {
        Console.WriteLine("FETCHING " + id);
        var getFileResponseDto = await _submissionService.DownloadFile(id);
        if (!getFileResponseDto.Success)
        {
            return NotFound(getFileResponseDto);
        }
        return File(fileContents: getFileResponseDto.Data!.FileByte, contentType: getFileResponseDto.Data!.ContentType, fileDownloadName: getFileResponseDto.Data!.fileDownloadName);
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSubmissionFile(int id)
    {
        Console.WriteLine("Deleting" + id);
        var deleteFile = await _submissionService.DeleteSubmissionAndFiles(id);
        if (!deleteFile.Success)
        {
            return NotFound(deleteFile);
        }

        return Ok(new { message = "Delete", id });

    }
}
