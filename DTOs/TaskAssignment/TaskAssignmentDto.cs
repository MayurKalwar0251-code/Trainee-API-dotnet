using System.ComponentModel.DataAnnotations;

namespace TrainineeAPI.Models;

public class TaskAssignmentDto
{
    public long Id { get; set; }
    public required long TraineeId { get; set; }
    public required long MentorId { get; set; }
    public required long LearningTaskId { get; set; }

    [Required]
    [EnumDataType(typeof(TaskAssignmentEnumValues),ErrorMessage = "Invalid Task Assignment Status Specified")]
    public required string Status { get; set; }
    public required string Remarks { get; set; }
    public DateOnly AssignedDate { get; set; }
    public DateOnly DueDate { get; set; }
    public DateOnly CreatedDate { get; set; }
    public DateOnly UpdatedDate { get; set; }
}