using System.ComponentModel.DataAnnotations;

namespace TrainineeAPI.Models;

public class CreateTaskAssignmentDto
{
    [Required]
    public required long TraineeId { get; set; }
    [Required]
    public required long MentorId { get; set; }
    [Required]
    public required long LearningTaskId { get; set; }
    [EnumDataType(typeof(TaskAssignmentEnumValues),ErrorMessage = "Invalid Task Assignment Status Specified")]
    public string Status { get; set; } = "Assigned";
    public string? Remarks { get; set; }
    public DateOnly AssignedDate { get; set; }
    public required DateOnly DueDate { get; set; }
}