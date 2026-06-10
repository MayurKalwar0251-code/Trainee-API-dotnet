using System.ComponentModel.DataAnnotations;
namespace TrainineeAPI.DTOs;

public class UserResponseDto
{
    public long Id { get; set; }
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required string Role { get; set; }
    public DateOnly CreatedDate { get; set; }
    public DateOnly UpdatedDate { get; set; }
}