namespace TrainineeAPI.Models;

public class SubmissionFile
{
    public long Id { get; set; }
    public required long SubmissionId { get; set; }
    public Submission? Submission {get; set;}
    public required long UserId { get; set; }
    public User? User {get; set;}
    public required string OriginalFileName { get; set; }
    public required string GeneratedStorageName { get; set; }
    public required string ContentType { get; set; }
    public required string Checksum { get; set; }
    public required long Size { get; set; }
    public DateOnly CreatedDate { get; set; }
    public DateOnly UpdatedDate { get; set; }
}