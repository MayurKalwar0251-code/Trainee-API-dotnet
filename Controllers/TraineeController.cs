using Microsoft.AspNetCore.Mvc;
using TrainineeAPI.Models;

namespace TrainineeAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class TraineeController : ControllerBase
{

    private static List<Trainee> Trainees {get; set;} = new List<Trainee> {};

    private readonly ILogger<TraineeController> _logger;

    public TraineeController(ILogger<TraineeController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public ActionResult<TraineeDto> GetAllTrainee()
    {
        var traineeDto = Trainees.Select(t => new TraineeDto
            {
                FirstName = t.FirstName,
                LastName = t.LastName,
                Email = t.Email,
                Status = t.Status,
                TechStack = t.TechStack,
                CreatedDate = t.CreatedDate,
                UpdatedDate = t.UpdatedDate,
            }
        );
        return Ok(traineeDto);
    }

    [HttpGet("{id}")]
    public ActionResult<TraineeDto> GetTraineeById(int id)
    {
        var traineeById = Trainees.FirstOrDefault(t => t.Id == id);

        if (traineeById == null)
        {
            return NotFound();
        }

        TraineeDto traineeDto = new TraineeDto{
            FirstName = traineeById.FirstName,
            LastName = traineeById.LastName,
            Email = traineeById.Email,
            Status = traineeById.Status,
            TechStack = traineeById.TechStack,
            CreatedDate = traineeById.CreatedDate,
            UpdatedDate = traineeById.UpdatedDate,
        };

        return Ok(traineeDto);
    }

    [HttpPost]
    public TraineeDTO CreateTrainee(Trainee trainee)
    {
        trainee.Id = Trainees.Count == 0 ? 1 : Trainees.Max(t => t.Id) + 1;
        Trainees.Add(trainee);
        return trainee;
    }

    [HttpPut("{id}")]
    public IActionResult UpdateTrainee(int id, Trainee updatedDetails)
    {
        // var traineeById = Trainees.FirstOrDefault(t => t.Id == id);

        // if (traineeById == null)
        // {
        //     return NotFound();
        // }

        // traineeById.Email = updatedDetails.Email;
        // traineeById.TechStack = updatedDetails.TechStack;
        // traineeById.FirstName = updatedDetails.FirstName;
        // traineeById.LastName = updatedDetails.LastName;
        // traineeById.Status = updatedDetails.Status;

        // return Ok(traineeById);

        var traineeIndex = Trainees.FindIndex(t => t.Id == id);

        if (traineeIndex == -1)
        {
            return NotFound();
        }

        updatedDetails.Id = id;
        Trainees[traineeIndex] = updatedDetails;
        return Ok(updatedDetails);
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteTrainee(int id)
    {
        var trainee = Trainees.FirstOrDefault(t => t.Id == id);

        if (trainee == null)
        {
            return NotFound();
        }

        Trainees.Remove(trainee);

        return Ok(new
        {
            StatusCode = 200,
            Message = "Deleted SUccessfully"
        });
    }

}
