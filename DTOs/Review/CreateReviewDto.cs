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

    [Required]
    [EnumDataType(typeof(ReviewStatusEnumValues),ErrorMessage = "Invalid Review Status Specified")]
    public required string ReviewStatus { get; set; }
}