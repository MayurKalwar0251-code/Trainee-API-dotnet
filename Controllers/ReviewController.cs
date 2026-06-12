using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrainineeAPI.DTOs;
using TrainineeAPI.Models;
using YamlDotNet.Core.Tokens;

namespace TrainineeAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class ReviewController : ControllerBase
{
    private readonly ILogger<ReviewController> _logger;
    private readonly IReviewService _reviewService;
    public ReviewController(ILogger<ReviewController> logger,IReviewService reviewService)
    {
        _logger = logger;
        _reviewService = reviewService;
    }

    [Authorize]
    [HttpGet("")]
    public async Task<ActionResult<IEnumerable<ReviewDto>>> GetAllReview([FromQuery] FilterTraineeDto filter)
    {
        try
        {
            var reviews = await _reviewService.GetAll();
            return Ok(reviews);
        }
        catch (System.Exception)
        {
            _logger.LogError(ErrorConstants.InternalServerError);
            return Problem(ErrorConstants.InternalServerError);
        }
    }

    [Authorize]
    [HttpGet("{id}")]
    public ActionResult<ReviewDto> GetReviewById(int id)
    {
        try
        {
            var reviewById = _reviewService.GetById(id);

            if (reviewById == null)
            {
                _logger.LogError(ErrorConstants.UserNotFound);
                return NotFound();
            }

            return Ok(reviewById);
        }
        catch (System.Exception)
        {
            _logger.LogError(ErrorConstants.InternalServerError);
            return Problem(ErrorConstants.InternalServerError);
        }
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<ReviewDto>> CreateReview(CreateReviewDto review)
    {
        try
        {
            Console.WriteLine("Review Creation Started");
            ServiceResult<ReviewDto> result = await _reviewService.Create(review);
            if (result == null)
            {
                return Ok(new
                {
                   Message = "Mentor Id or Submission Id docs are not found" 
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
