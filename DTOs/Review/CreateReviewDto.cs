using System.ComponentModel.DataAnnotations;

namespace TrainineeAPI.Models;

public class CreateReviewDto
{
    [Required]
    public required long SubmissionId { get; set; }
    [Required]
    public required long MentorId { get; set; }
    [Required]
    public required string Feedback { get; set; }
    public int? Score { get; set; }

    [EnumDataType(typeof(ReviewStatusEnumValues),ErrorMessage = "Invalid Review Status Specified")]
    public string ReviewStatus { get; set; } = "Draft";
}