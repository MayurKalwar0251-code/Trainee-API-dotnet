using System.ComponentModel.DataAnnotations;

namespace TrainineeAPI.Models;

public class ReviewDto
{
    public long Id { get; set; }
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
    public DateOnly ReviewedDate { get; set; }
    public DateOnly CreatedDate { get; set; }
    public DateOnly UpdatedDate { get; set; }
}