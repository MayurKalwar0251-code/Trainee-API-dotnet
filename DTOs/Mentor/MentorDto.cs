using System.ComponentModel.DataAnnotations;

namespace TrainineeAPI.Models;

public class MentorDto
{
    public long Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required string Expertise { get; set; }

    [Required]
    [EnumDataType(typeof(MentorStatusEnumValues),ErrorMessage = "Invalid Mentor Status Specified")]
    public required string Status { get; set; }
    public DateOnly CreatedDate { get; set; }
    public DateOnly UpdatedDate { get; set; }
}