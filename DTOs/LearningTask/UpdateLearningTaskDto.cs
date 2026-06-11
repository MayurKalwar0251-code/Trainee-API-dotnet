using System.ComponentModel.DataAnnotations;

namespace TrainineeAPI.Models;

public class UpdateLearningTaskDto
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? ExpectedTechStack { get; set; }
    
    [Required]
    [EnumDataType(typeof(LearningTaskStatusEnumValues),ErrorMessage = "Invalid Learning Task Status Specified")]
    public string? Status { get; set; }
    public DateOnly DueDate { get; set; }
}