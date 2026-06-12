using TrainineeAPI.DTOs;
using TrainineeAPI.Models;

public interface IReviewService
{
    Task<ServiceResult<List<ReviewDto>>> GetAll();

    ServiceResult<ReviewDto> GetById(int id);

    Task<ServiceResult<ReviewDto>> Create(CreateReviewDto dto);
}
