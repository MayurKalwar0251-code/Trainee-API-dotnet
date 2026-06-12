using System.ComponentModel.DataAnnotations;

namespace TrainineeAPI.Models;

public class SubmissionDto
{
    public long Id { get; set; }
    public required long TaskAssignmentId { get; set; }
    public required string SubmissionUrl { get; set; }
    public string? Notes { get; set; }

    [EnumDataType(typeof(SubmissionStatusEnumValues),ErrorMessage = "Invalid Submission Status Specified")]
    public string? Status { get; set; }
    public DateOnly SubmittedDate { get; set; }
    public DateOnly CreatedDate { get; set; }
    public DateOnly UpdatedDate { get; set; }
}