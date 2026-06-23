namespace TrainineeAPI.Models;
public class SubmissionProcessingRequestModel
{
    public string MessageId { get; set; } = Guid.NewGuid().ToString();
    public string CorrelationId { get; set; } = Guid.NewGuid().ToString();
    public required long SubmissionId { get; set; }
    public Submission? Submission { get; set; }
    public required long SubmissionFileId { get; set; }
    public SubmissionFile? SubmissionFile { get; set; }
    public DateOnly RequestedAt {get; set;} = DateOnly.FromDateTime(DateTime.Now);
    public string? ContractVersion {get; set;}
}