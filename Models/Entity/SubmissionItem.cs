namespace TrainineeAPI.Models;

public enum SubmissionStatusEnumValues {Submitted , Resubmitted};

public class Submission
{
    public long Id { get; set; }
    public required long TaskAssignmentId { get; set; }
    public TaskAssignment? TaskAssignment {get; set;}
    public required string SubmissionUrl { get; set; }
    public required string Notes { get; set; }
    public required string Status { get; set; }
    public DateOnly SubmittedDate { get; set; }
    public DateOnly CreatedDate { get; set; }
    public DateOnly UpdatedDate { get; set; }
}