using System.ComponentModel.DataAnnotations;

namespace TrainineeAPI.Models;

public class CreateSubmissionDto
{
    [Required]
    public required long TaskAssignmentId { get; set; }
    [Required]
    public required string SubmissionUrl { get; set; }
    public string? Notes { get; set; }

    [EnumDataType(typeof(SubmissionStatusEnumValues),ErrorMessage = "Invalid Submission Status Specified")]
    public string? Status { get; set; }
}