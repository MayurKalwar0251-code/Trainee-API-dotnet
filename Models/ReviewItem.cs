namespace TrainineeAPI.Models;

public enum ReviewStatusEnumValues {Draft , Published , Closed};

public class Review
{
    public long Id { get; set; }
    public required long SubmissionId { get; set; }
    public Submission? Submission {get; set;}
    public required long MentorId { get; set; }
    public Mentor? Mentor {get; set;}
    public required string Feedback { get; set; }
    public int? Score { get; set; }
    public required string ReviewStatus { get; set; }
    public DateOnly ReviewedDate { get; set; }
    public DateOnly CreatedDate { get; set; }
    public DateOnly UpdatedDate { get; set; }
}