using System.ComponentModel.DataAnnotations;

public class SubmitSubmissionFileDto
{
    public required IFormFileCollection Files { get; set; }
}