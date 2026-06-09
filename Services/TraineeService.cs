using Microsoft.EntityFrameworkCore;
using TrainineeAPI.DTOs;
using TrainineeAPI.Models;

public class TraineeService : ITraineeService
{
    private readonly TraineeContext _traineeContext;

    public TraineeService(TraineeContext traineeContext)
    {
        _traineeContext = traineeContext;
    }

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

    public async Task<List<TraineeDto>> GetAll()
    {
        var trainees = await _traineeContext.Trainees.ToListAsync();

        var traineeDtos = trainees
            .Select(t => ConvertToTraineeDTOResponse(t))
            .ToList();

        return traineeDtos;
    }

    public TraineeDto? GetById(int id)
    {
        var traineeById = _traineeContext.Trainees.FirstOrDefault(t => t.Id == id);

        if (traineeById == null)
        {
            return null;
        }

        TraineeDto traineeDto = ConvertToTraineeDTOResponse(traineeById);

        return traineeDto;
    }

    public TraineeDto Create(CreateTraineeDto trainee)
    {
        var id = _traineeContext.Trainees.Count() == 0 ? 1 : _traineeContext.Trainees.Max(t => t.Id) + 1;

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
            FirstName = trainee.FirstName!,
            LastName = trainee.LastName!,
            Email = trainee.Email!,
            Status = trainee.Status,
            TechStack = trainee.TechStack!,
            CreatedDate = DateOnly.FromDateTime(DateTime.Now),
            UpdatedDate = DateOnly.FromDateTime(DateTime.Now),
        };

        _traineeContext.Trainees.Add(newTrainee);

        _traineeContext.SaveChangesAsync();

        return traineeDto;
    }

    public async Task<bool> Delete(int id)
    {
        var trainee = _traineeContext.Trainees.FirstOrDefault(t => t.Id == id);

        if (trainee == null)
        {
            return false;
        }

        _traineeContext.Trainees.Remove(trainee);

        await _traineeContext.SaveChangesAsync();

        return true;
    }

    public async Task<TraineeDto?> Update(int id, UpdateTraineeDto updatedDetails)
    {
        var trainee = _traineeContext.Trainees.FirstOrDefault(t => t.Id == id);

        if (trainee == null)
        {
            return null;
        }

        trainee.FirstName = updatedDetails.FirstName!;
        trainee.LastName = updatedDetails.LastName!;
        trainee.Email = updatedDetails.Email!;
        trainee.Status = updatedDetails.Status;
        trainee.TechStack = updatedDetails.TechStack!;
        trainee.UpdatedDate = DateOnly.FromDateTime(DateTime.Now);

        await _traineeContext.SaveChangesAsync();

        var response = new TraineeDto
        {
            FirstName = trainee.FirstName,
            LastName = trainee.LastName,
            Email = trainee.Email,
            Status = trainee.Status,
            TechStack = trainee.TechStack,
            CreatedDate = trainee.CreatedDate,
            UpdatedDate = trainee.UpdatedDate
        };

        return response;
    }

    public async Task<List<TraineeDto>> FilterBySearch(string search)
    {
        var filterResult = await _traineeContext.Trainees.Where(item => item.FirstName.Contains(search) 
                                                                    || item.LastName.Contains(search) 
                                                                    || item.Email.Contains(search) 
                                                                    || item.TechStack.Contains(search))
                                                                    .ToListAsync();

        var traineeDtos = filterResult.Select(item => ConvertToTraineeDTOResponse(item)).ToList();

        return traineeDtos;
    }
}