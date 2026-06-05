namespace TrainineeAPI.DTOs;

public class CreateTraineeDto
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? TechStack { get; set; }
    public bool Status { get; set; }
}