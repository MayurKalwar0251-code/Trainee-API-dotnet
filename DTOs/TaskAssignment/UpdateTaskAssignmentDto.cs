using System.ComponentModel.DataAnnotations;

namespace TrainineeAPI.Models;

public class UpdateTaskAssignmentDto
{
    public required long TraineeId { get; set; }
    public required long MentorId { get; set; }
    public required long LearningTaskId { get; set; }

    [Required]
    [EnumDataType(typeof(TaskAssignmentEnumValues),ErrorMessage = "Invalid Task Assignment Status Specified")]
    public string? Status { get; set; }
    public string? Remarks { get; set; }
    public DateOnly AssignedDate { get; set; }
    public DateOnly DueDate { get; set; }
}