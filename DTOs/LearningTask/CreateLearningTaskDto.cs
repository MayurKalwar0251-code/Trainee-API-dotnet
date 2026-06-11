using System.ComponentModel.DataAnnotations;

namespace TrainineeAPI.Models;

public class CreateLearningTaskDto
{
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required string ExpectedTechStack { get; set; }
    
    [Required]
    [EnumDataType(typeof(LearningTaskStatusEnumValues),ErrorMessage = "Invalid Learning Task Status Specified")]
    public required string Status { get; set; }
    public DateOnly DueDate { get; set; }
}