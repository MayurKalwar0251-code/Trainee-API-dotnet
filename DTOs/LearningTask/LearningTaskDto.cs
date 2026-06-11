using System.ComponentModel.DataAnnotations;

namespace TrainineeAPI.Models;

public class LearningTaskDto
{
    public long Id { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required string ExpectedTechStack { get; set; }
    
    [Required]
    [EnumDataType(typeof(LearningTaskStatusEnumValues),ErrorMessage = "Invalid Learning Task Status Specified")]
    public required string Status { get; set; }
    public DateOnly DueDate { get; set; }
    public DateOnly CreatedDate { get; set; }
    public DateOnly UpdatedDate { get; set; }
}