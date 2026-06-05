using Microsoft.AspNetCore.Mvc;
using TrainineeAPI.Models;
using TrainineeAPI.DTOs;

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

    [NonAction]
    public TraineeDto ConvertToTraineeDTOResponse(Trainee data)
    {
        TraineeDto converted = new TraineeDto
        {
            FirstName = data.FirstName,
            LastName = data.LastName,
            Email = data.Email,
            Status = data.Status,
            TechStack = data.TechStack,
            CreatedDate = data.CreatedDate,
            UpdatedDate = data.UpdatedDate,
        };
        return converted;
    }

    [HttpGet]
    public ActionResult<IEnumerable<TraineeDto>> GetAllTrainee()
    {
        var traineeDtos = Trainees
            .Select(t => ConvertToTraineeDTOResponse(t))
            .ToList();

        return Ok(traineeDtos);
    }

    [HttpGet("{id}")]
    public ActionResult<TraineeDto> GetTraineeById(int id)
    {
        var traineeById = Trainees.FirstOrDefault(t => t.Id == id);

        if (traineeById == null)
        {
            return NotFound();
        }

        TraineeDto traineeDto = ConvertToTraineeDTOResponse(traineeById);

        return Ok(traineeDto);
    }

    [HttpPost]
    public ActionResult<TraineeDto> CreateTrainee(CreateTraineeDto trainee)
    {
        var id = Trainees.Count == 0 ? 1 : Trainees.Max(t => t.Id) + 1;

        var traineeDto = new TraineeDto
        {
            FirstName = trainee.FirstName,  
            LastName = trainee.LastName,
            Email = trainee.Email,
            Status = trainee.Status,
            TechStack = trainee.TechStack,
            CreatedDate = DateOnly.FromDateTime(DateTime.Now),
            UpdatedDate = DateOnly.FromDateTime(DateTime.Now),
        };

        Trainee newTrainee = new Trainee
        {   
            Id = id,
            FirstName = trainee.FirstName,  
            LastName = trainee.LastName,
            Email = trainee.Email,
            Status = trainee.Status,
            TechStack = trainee.TechStack,
            CreatedDate = DateOnly.FromDateTime(DateTime.Now),
            UpdatedDate = DateOnly.FromDateTime(DateTime.Now),
        };

        Trainees.Add(newTrainee);

        return Ok(traineeDto);
    }

    [HttpPut("{id}")]
    public ActionResult<TraineeDto> UpdateTrainee(int id, UpdateTraineeDto updatedDetails)
    {
        var traineeIndex = Trainees.FindIndex(t => t.Id == id);

        if (traineeIndex == -1)
        {
            return NotFound();
        }

        Trainee oldata = Trainees[traineeIndex];

        Trainee updatedTrainee = new Trainee
        {
            Id = id,
            FirstName = updatedDetails.FirstName,  
            LastName = updatedDetails.LastName,
            Email = updatedDetails.Email,
            Status = updatedDetails.Status,
            TechStack = updatedDetails.TechStack,
            UpdatedDate = DateOnly.FromDateTime(DateTime.Now),
            CreatedDate = oldata.CreatedDate
        };

        Trainees[traineeIndex] = updatedTrainee;

        var response = new TraineeDto
        {
            FirstName = updatedTrainee.FirstName,
            LastName = updatedTrainee.LastName,
            Email = updatedTrainee.Email,
            Status = updatedTrainee.Status,
            TechStack = updatedTrainee.TechStack,
            CreatedDate = updatedTrainee.CreatedDate,
            UpdatedDate = updatedTrainee.UpdatedDate
        };

        return Ok(response);
    }


    [HttpPatch("{id}")]
    public ActionResult<TraineeDto> UpdateTraineeUsingPatch(int id, UpdateTraineeDto updatedDetails)
    {
        var traineeIndex = Trainees.FindIndex(t => t.Id == id);

        if (traineeIndex == -1)
        {
            return NotFound();
        }

        Trainee olddata = Trainees[traineeIndex];

        olddata.Id = id;
        olddata.FirstName = updatedDetails.FirstName;
        olddata.LastName = updatedDetails.LastName;
        olddata.Status = updatedDetails.Status;
        olddata.TechStack = updatedDetails.TechStack;
        olddata.UpdatedDate = DateOnly.FromDateTime(DateTime.Now);

        return Ok();
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
