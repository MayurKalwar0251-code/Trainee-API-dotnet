using System.ComponentModel.DataAnnotations;

namespace TrainineeAPI.Models;

public class CreateLearningTaskDto
{
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required string ExpectedTechStack { get; set; }
    
    [EnumDataType(typeof(LearningTaskStatusEnumValues),ErrorMessage = "Invalid Learning Task Status Specified")]
    public string Status { get; set; } = "Draft";
    public DateOnly DueDate { get; set; }
}