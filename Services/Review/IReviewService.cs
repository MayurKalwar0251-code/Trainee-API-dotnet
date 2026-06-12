using TrainineeAPI.DTOs;
using TrainineeAPI.Models;

public interface IReviewService
{
    Task<List<ReviewDto>> GetAll();

    ReviewDto? GetById(int id);

    Task<ReviewDto?> Create(CreateReviewDto dto);
}
