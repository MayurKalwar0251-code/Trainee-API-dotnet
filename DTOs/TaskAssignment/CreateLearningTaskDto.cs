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
    [Required]
    [EnumDataType(typeof(TaskAssignmentEnumValues),ErrorMessage = "Invalid Task Assignment Status Specified")]
    public required string Status { get; set; }
    public required string Remarks { get; set; }
    public required DateOnly AssignedDate { get; set; }
    public required DateOnly DueDate { get; set; }
}