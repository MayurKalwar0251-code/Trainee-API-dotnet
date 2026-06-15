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
    public ReviewController(ILogger<ReviewController> logger, IReviewService reviewService)
    {
        _logger = logger;
        _reviewService = reviewService;
    }

    [Authorize]
    [HttpGet("")]
    public async Task<ActionResult<IEnumerable<ReviewDto>>> GetAllReview()
    {
        var reviews = await _reviewService.GetAll();
        return Ok(reviews);
    }

    [Authorize]
    [HttpGet("{id}")]
    public ActionResult<ReviewDto> GetReviewById(int id)
    {
        ServiceResult<ReviewDto> reviewById = _reviewService.GetById(id);

        if (reviewById.Data == null)
        {
            _logger.LogError(ErrorConstants.DocumentNotFound);
            return NotFound(reviewById);
        }

        return Ok(reviewById);
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<ReviewDto>> CreateReview(CreateReviewDto review)
    {
        Console.WriteLine("Review Creation Started");
        ServiceResult<ReviewDto> result = await _reviewService.Create(review);
        if (result.Data == null)
        {
            _logger.LogError(ErrorConstants.DocumentNotFound);
            return NotFound(result);
        }
        _logger.LogInformation(MessagesConstants.CreatedSuccessfully);
        return Ok(result);
    }
}
