namespace TrainineeAPI.DTOs;

public class FilterTraineeDto
{
    public string? Search { get; set; }
    public int? PageNumber {get; set;}
    public int? PageSize {get; set;}
    // TODO Status coversion to enum
    public bool? Status {get; set;}
    
}