namespace TrainineeAPI.Models;

public enum LearningTaskStatusEnumValues {Draft , Published , Closed};

public class LearningTask
{
    public long Id { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required string ExpectedTechStack { get; set; }
    public required string Status { get; set; }
    public DateOnly DueDate { get; set; }
    public DateOnly CreatedDate { get; set; }
    public DateOnly UpdatedDate { get; set; }
}