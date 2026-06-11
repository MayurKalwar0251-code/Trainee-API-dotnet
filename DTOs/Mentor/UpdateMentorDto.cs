using System.ComponentModel.DataAnnotations;

namespace TrainineeAPI.Models;

public class UpdateMentorDto
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? Expertise { get; set; }

    [EnumDataType(typeof(MentorStatusEnumValues),ErrorMessage = "Invalid Mentor Status Specified")]
    public string? Status { get; set; }
}