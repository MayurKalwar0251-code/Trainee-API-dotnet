namespace TrainineeAPI.Models;

public class TaskAssignment
{
    public long Id { get; set; }
    public required long TraineeId { get; set; }
    public Trainee? Trainee { get; set; }
    public required long MentorId { get; set; }
    public Mentor? Mentor { get; set; }
    public required long LearningTaskId { get; set; }
    public LearningTask? LearningTask { get; set; }
    public required string Status { get; set; }
    public required string Remarks { get; set; }
    public DateOnly AssignedDate { get; set; }
    public DateOnly DueDate { get; set; }
    public DateOnly CreatedDate { get; set; }
    public DateOnly UpdatedDate { get; set; }
}