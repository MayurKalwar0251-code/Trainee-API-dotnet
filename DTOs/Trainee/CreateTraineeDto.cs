using System.ComponentModel.DataAnnotations;
using TrainineeAPI.Models;

namespace TrainineeAPI.DTOs;

public class CreateTraineeDto
{
    [Required]
    [MinLength(1, ErrorMessage = "Name should be of minimum 1 length")]
    public string? FirstName { get; set; }

    [Required]
    [MinLength(1, ErrorMessage = "Name should be of minimum 1 length")]
    public string? LastName { get; set; }

    [EmailAddress]
    public string? Email { get; set; }

    [Required]
    public required string Password {get; set;}

    public string? TechStack { get; set; }

    [EnumDataType(typeof(TraineeStatusEnumValues),ErrorMessage = "Invalid Status Value Specified")]
    public string Status { get; set; } = "Active";
}