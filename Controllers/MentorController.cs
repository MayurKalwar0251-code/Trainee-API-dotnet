using System.Threading.Tasks;
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
    public MentorController(ILogger<MentorController> logger, IMentorService mentorService)
    {
        _logger = logger;
        _mentorService = mentorService;
    }

    [Authorize]
    [HttpGet("")]
    public async Task<ActionResult<IEnumerable<MentorDto>>> GetAllMentor()
    {
        var mentors = await _mentorService.GetAll();
        return Ok(mentors);
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<ActionResult<MentorDto>> GetMentorById(int id)
    {
        var mentorById = await _mentorService.GetById(id);

        if (mentorById == null)
        {
            _logger.LogError(ErrorConstants.DocumentNotFound);
            return NotFound();
        }

        return Ok(mentorById);
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<MentorDto>> CreateMentor(CreateMentorDto mentor)
    {
        Console.WriteLine("Mentor Creation Started");
        var result = await _mentorService.Create(mentor);
        _logger.LogInformation(MessagesConstants.CreatedSuccessfully);
        return Ok(result);
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<ActionResult<MentorDto>> UpdateMentor(int id, UpdateMentorDto updatedDetails)
    {
        var updateMentor = await _mentorService.Update(id, updatedDetails);

        if (updateMentor == null)
        {
            _logger.LogError(ErrorConstants.DocumentNotFound);
            return NotFound();
        }
        _logger.LogInformation(MessagesConstants.UpdatedSuccessfully);
        return Ok(updateMentor);
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMentor(int id)
    {
        var deleteStatus = await _mentorService.Delete(id);

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
