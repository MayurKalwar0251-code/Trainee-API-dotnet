using System.ComponentModel.DataAnnotations;

namespace TrainineeAPI.DTOs;

public class CreateTraineeDto
{
    [Required]
    [MinLength(1, ErrorMessage = "Name should be of minimum 1 length")]
    public string? FirstName { get; set; }

    [Required]
    [MinLength(1, ErrorMessage = "Name should be of minimum 1 length")]
    public string? LastName { get; set; }

    [Required]
    [EmailAddress]
    public string? Email { get; set; }

    [Required]
    [EnumDataType(typeof(TechStackEnumValues),ErrorMessage = "Invalid TechStack Specified")]
    public string? TechStack { get; set; }
    public bool Status { get; set; }
}