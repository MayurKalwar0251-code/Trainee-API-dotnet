using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using TrainineeAPI.DTOs;
using TrainineeAPI.Models;

public class TraineeService : ITraineeService
{
    private readonly TraineeContext _traineeContext;
    private readonly IMapper _mapper;
    private readonly ICacheService _cacheService;

    public TraineeService(TraineeContext traineeContext, IMapper mapper, ICacheService cacheService)
    {
        _traineeContext = traineeContext;
        _mapper = mapper;
        _cacheService = cacheService;
    }
    public async Task<ServiceResult<List<TraineeDto>>> GetAll()
    {
        var trainees = await _traineeContext.Trainees.ToListAsync();

        var traineeDtos = trainees
            .Select(t => _mapper.Map<TraineeDto>(t))
            .ToList();

        return ServiceResult<List<TraineeDto>>.Ok(traineeDtos);
    }

    public async Task<ServiceResult<TraineeDto>> GetById(int id)
    {
        string key = $"trainee:{id}";
        Console.WriteLine("KEY : " + key);

        var data = await _cacheService.GetAsync<TraineeDto>(key);

        if (data != null)
        {
            Console.WriteLine("Fetched from Cachec : " + key);
            return ServiceResult<TraineeDto>.Ok(data);
        }
        Console.WriteLine("Fetching from db : " + key);
        var traineeById = _traineeContext.Trainees.FirstOrDefault(t => t.Id == id);

        if (traineeById == null)
        {
            return ServiceResult<TraineeDto>.Fail(ErrorConstants.DocumentNotFound);
        }

        TraineeDto traineeDto = _mapper.Map<TraineeDto>(traineeById);

        var cacheOptions = new DistributedCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(20));

        await _cacheService.SetAsync(key,traineeDto,cacheOptions);

        return ServiceResult<TraineeDto>.Ok(traineeDto);
    }

    public async Task<ServiceResult<TraineeDto>> Create(CreateTraineeDto trainee)
    {
        // check email already exists
        var existsUser = await _traineeContext.Users.FirstOrDefaultAsync(x => x.Email == trainee.Email);

        if (existsUser != null)
        {
            return ServiceResult<TraineeDto>.Fail("User Already Exists");
        } 

        var user = new User
        {
            Id = _traineeContext.Users.Count() == 0 ? 1 : _traineeContext.Users.Max(t => t.Id) + 1,
            Username = trainee.FirstName!,
            Email = trainee.Email!,
            PasswordHash = PasswordUtility.HashUserPassword(trainee.Password),
            Role = "Trainee",
            CreatedDate = DateOnly.FromDateTime(DateTime.Now),
            UpdatedDate = DateOnly.FromDateTime(DateTime.Now)
        };

        Console.WriteLine(JsonSerializer.Serialize(user));

        await _traineeContext.Users.AddAsync(user);

        await _traineeContext.SaveChangesAsync();

        var id = _traineeContext.Trainees.Count() == 0 ? 1 : _traineeContext.Trainees.Max(t => t.Id) + 1;

        var traineeCreated = new Trainee
        {
            Id = id,
            UserId = user.Id,
            FirstName = trainee.FirstName!,
            LastName = trainee.LastName!,
            Email = trainee.Email!,
            TechStack = trainee.TechStack!,
            Status = "Active",
            CreatedDate = DateOnly.FromDateTime(DateTime.Now),
            UpdatedDate = DateOnly.FromDateTime(DateTime.Now),
        };

        await _traineeContext.Trainees.AddAsync(traineeCreated);

        Console.WriteLine(JsonSerializer.Serialize(traineeCreated));

        await _traineeContext.SaveChangesAsync();

        var traineeDto = _mapper.Map<TraineeDto>(traineeCreated);

        return ServiceResult<TraineeDto>.Ok(traineeDto);
    }

    public async Task<ServiceResult<bool>> Delete(int id)
    {
        var trainee = _traineeContext.Trainees.FirstOrDefault(t => t.Id == id);

        if (trainee == null)
        {
            return ServiceResult<bool>.Fail(ErrorConstants.DocumentNotFound);
        }

        _traineeContext.Trainees.Remove(trainee);

        await _traineeContext.SaveChangesAsync();

        string key = $"trainee:{id}";

        Console.WriteLine("Deleted from Cachec : " + key);
        await _cacheService.RemoveAsync(key);

        return ServiceResult<bool>.Ok(true);
    }

    public async Task<ServiceResult<TraineeDto>> Update(int id, UpdateTraineeDto updatedDetails)
    {
        var trainee = _traineeContext.Trainees.FirstOrDefault(t => t.Id == id);

        if (trainee == null)
        {
            return ServiceResult<TraineeDto>.Fail(ErrorConstants.DocumentNotFound);
        }

        _mapper.Map(updatedDetails,trainee);
        trainee.Id = id;
        trainee.UpdatedDate = DateOnly.FromDateTime(DateTime.Now);

        await _traineeContext.SaveChangesAsync();

        var response = _mapper.Map<TraineeDto>(trainee);

        string key = $"trainee:{id}";
        await _cacheService.RemoveAsync(key);

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
    public async Task<ServiceResult<PagedResponse<TraineeDto>>> FilterByQuery(FilterTraineeDto filter)
    {
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

        if (!string.IsNullOrEmpty(filter.Status))
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

        PagedResponse<TraineeDto> pagedResponse = new PagedResponse<TraineeDto>
        {
            Items = resultDTO,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalDocs,
        };

        return ServiceResult<PagedResponse<TraineeDto>>.Ok(pagedResponse);
    }
}