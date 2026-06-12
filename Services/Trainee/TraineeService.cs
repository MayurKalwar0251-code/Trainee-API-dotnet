using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TrainineeAPI.DTOs;
using TrainineeAPI.Models;

public class TraineeService : ITraineeService
{
    private readonly TraineeContext _traineeContext;
    private readonly IMapper _mapper;

    public TraineeService(TraineeContext traineeContext, IMapper mapper)
    {
        _traineeContext = traineeContext;
        _mapper = mapper;
    }
    public async Task<ServiceResult<List<TraineeDto>>> GetAll()
    {
        var trainees = await _traineeContext.Trainees.ToListAsync();

        var traineeDtos = trainees
            .Select(t => _mapper.Map<TraineeDto>(t))
            // .Select(t => ConvertToTraineeDTOResponse(t))
            .ToList();

        return ServiceResult<List<TraineeDto>>.Ok(traineeDtos);
    }

    public ServiceResult<TraineeDto> GetById(int id)
    {
        var traineeById = _traineeContext.Trainees.FirstOrDefault(t => t.Id == id);

        if (traineeById == null)
        {
            return ServiceResult<TraineeDto>.Fail("Document not found");
        }

        TraineeDto traineeDto = _mapper.Map<TraineeDto>(traineeById);

        return ServiceResult<TraineeDto>.Ok(traineeDto);
    }

    public ServiceResult<TraineeDto> Create(CreateTraineeDto trainee)
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

        return ServiceResult<TraineeDto>.Ok(traineeDto);
    }

    public async Task<ServiceResult<bool>> Delete(int id)
    {
        var trainee = _traineeContext.Trainees.FirstOrDefault(t => t.Id == id);

        if (trainee == null)
        {
            return ServiceResult<bool>.Fail("Document not found");
        }

        _traineeContext.Trainees.Remove(trainee);

        await _traineeContext.SaveChangesAsync();

        return ServiceResult<bool>.Ok(true);
    }

    public async Task<ServiceResult<TraineeDto>> Update(int id, UpdateTraineeDto updatedDetails)
    {
        var trainee = _traineeContext.Trainees.FirstOrDefault(t => t.Id == id);

        if (trainee == null)
        {
            return ServiceResult<TraineeDto>.Fail("Document not found");
        }

        trainee.FirstName = updatedDetails.FirstName!;
        trainee.LastName = updatedDetails.LastName!;
        trainee.Email = updatedDetails.Email!;
        trainee.Status = updatedDetails.Status;
        trainee.TechStack = updatedDetails.TechStack!;
        trainee.UpdatedDate = DateOnly.FromDateTime(DateTime.Now);

        await _traineeContext.SaveChangesAsync();

        var response = _mapper.Map<TraineeDto>(trainee);

        return ServiceResult<TraineeDto>.Ok(response);
    }

    public async Task<ServiceResult<List<TraineeDto>>> FilterBySearch(string search)
    {
        var filterResult = await _traineeContext.Trainees.Where(item => item.FirstName.Contains(search)
                                                                    || item.LastName.Contains(search)
                                                                    || item.Email.Contains(search)
                                                                    || item.TechStack.Contains(search))
                                                                    .ToListAsync();

        var traineeDtos = filterResult.Select(item => _mapper.Map<TraineeDto>(item)).ToList();

        return ServiceResult<List<TraineeDto>>.Ok(traineeDtos);
    }
    public async Task<ServiceResult<List<TraineeDto>>> FilterByQuery(FilterTraineeDto filter)
    {
        Console.WriteLine("FILTER PARAM : " + filter);
        Console.WriteLine("FILTER PARAM Search : " + filter.Search);
        Console.WriteLine("FILTER PARAM NO: " + filter.PageNumber);
        Console.WriteLine("FILTER PARAM Size: " + filter.PageSize);
        Console.WriteLine("FILTER PARAM Status: " + filter.Status);

        IQueryable<Trainee> filterResult = _traineeContext.Trainees;

        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            filterResult = filterResult.Where(item => item.FirstName.Contains(filter.Search)
                                                                        || item.LastName.Contains(filter.Search)
                                                                        || item.Email.Contains(filter.Search)
                                                                        || item.TechStack.Contains(filter.Search)
                                                                        );

            Console.WriteLine("COUNT SearchFIlter : " + filterResult.Count());
        }

        if (filter.Status.HasValue)
        {
            filterResult = filterResult.Where(item => item.Status == filter.Status);

            Console.WriteLine("COUNT StatusFIlter : " + filterResult.Count());
        }

        // pagination logic
        int pageNumber = (int)(filter.PageNumber > 0 ? filter.PageNumber : 1);
        int pageSize = (int)(filter.PageSize > 0 ? filter.PageSize : 10);
        int totalDocs = filterResult.Count();
        filterResult = filterResult.Skip((pageNumber - 1) * pageSize).Take(pageSize);

        Console.WriteLine("COUNT PaginationFilter : " + filterResult.Count());

        List<Trainee> result = await filterResult.ToListAsync();

        List<TraineeDto> resultDTO = result.Select(item => _mapper.Map<TraineeDto>(item)).ToList();

        return ServiceResult<List<TraineeDto>>.Ok(resultDTO);
    }
}