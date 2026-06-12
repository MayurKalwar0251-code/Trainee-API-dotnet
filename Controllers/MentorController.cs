using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrainineeAPI.DTOs;
using TrainineeAPI.Models;
using YamlDotNet.Core.Tokens;

namespace TrainineeAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class MentorController : ControllerBase
{
    private readonly ILogger<MentorController> _logger;
    private readonly IMentorService _mentorService;
    public MentorController(ILogger<MentorController> logger,IMentorService mentorService)
    {
        _logger = logger;
        _mentorService = mentorService;
    }

    [Authorize]
    [HttpGet("")]
    public async Task<ActionResult<IEnumerable<MentorDto>>> GetAllMentor([FromQuery] FilterTraineeDto filter)
    {
        try
        {
            var mentors = await _mentorService.GetAll();
            return Ok(mentors);
        }
        catch (System.Exception)
        {
            _logger.LogError(ErrorConstants.InternalServerError);
            return Problem(ErrorConstants.InternalServerError);
        }
    }

    [Authorize]
    [HttpGet("{id}")]
    public ActionResult<MentorDto> GetMentorById(int id)
    {
        try
        {
            var mentorById = _mentorService.GetById(id);

            if (mentorById == null)
            {
                _logger.LogError(ErrorConstants.UserNotFound);
                return NotFound();
            }

            return Ok(mentorById);
        }
        catch (System.Exception)
        {
            _logger.LogError(ErrorConstants.InternalServerError);
            return Problem(ErrorConstants.InternalServerError);
        }
    }

    [Authorize]
    [HttpPost]
    public ActionResult<MentorDto> CreateMentor(CreateMentorDto mentor)
    {
        try
        {
            Console.WriteLine("Mentor Creation Started");
            var result = _mentorService.Create(mentor);
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
    public async Task<ActionResult<MentorDto>> UpdateMentor(int id, UpdateMentorDto updatedDetails)
    {
        try
        {
            var updateMentor = await _mentorService.Update(id,updatedDetails);

            if (updateMentor == null)
            {
                _logger.LogError(ErrorConstants.UserNotFound);
                return NotFound();
            }
            _logger.LogInformation(MessagesConstants.UpdatedSuccessfully);
            return Ok(updateMentor);
        }
        catch (System.Exception)
        {
            _logger.LogError(ErrorConstants.InternalServerError);
            return Problem(ErrorConstants.InternalServerError);
        }
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMentor(int id)
    {
        try
        {
            var deleteStatus = await _mentorService.Delete(id);

            if (!deleteStatus.Data)
            {
                _logger.LogError(ErrorConstants.UserNotFound);
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
