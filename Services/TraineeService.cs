using TrainineeAPI.DTOs;
using TrainineeAPI.Models;

public class TraineeService : ITraineeService
{
    private static List<Trainee> Trainees { get; set; } = new List<Trainee>
    {
    };

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

    public List<TraineeDto> GetAll()
    {
        var traineeDtos = Trainees
            .Select(t => ConvertToTraineeDTOResponse(t))
            .ToList();

        return traineeDtos;
    }

    public TraineeDto? GetById(int id)
    {
        var traineeById = Trainees.FirstOrDefault(t => t.Id == id);

        if (traineeById == null)
        {
            return null;
        }

        TraineeDto traineeDto = ConvertToTraineeDTOResponse(traineeById);

        return traineeDto;
    }

    public TraineeDto Create(CreateTraineeDto trainee)
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

        return traineeDto;
    }

    public bool Delete(int id)
    {
        var trainee = Trainees.FirstOrDefault(t => t.Id == id);

        if (trainee == null)
        {
            return false;
        }

        Trainees.Remove(trainee);

        return true;
    }

    public TraineeDto? Update(int id, UpdateTraineeDto updatedDetails)
    {
        var traineeIndex = Trainees.FindIndex(t => t.Id == id);

        if (traineeIndex == -1)
        {
            return null;
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

        return response;
    }

    public TraineeDto? UpdateUsingPatch(int id, UpdateTraineeDto updatedDetails)
    {
        var traineeIndex = Trainees.FindIndex(t => t.Id == id);

        if (traineeIndex == -1)
        {
            return null;
        }

        Trainee olddata = Trainees[traineeIndex];

        olddata.Id = id;
        olddata.FirstName = updatedDetails.FirstName;
        olddata.LastName = updatedDetails.LastName;
        olddata.Status = updatedDetails.Status;
        olddata.TechStack = updatedDetails.TechStack;
        olddata.UpdatedDate = DateOnly.FromDateTime(DateTime.Now);

        TraineeDto response = ConvertToTraineeDTOResponse(olddata);

        return response;
    }
}