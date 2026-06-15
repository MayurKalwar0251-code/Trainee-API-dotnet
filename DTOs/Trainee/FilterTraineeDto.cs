using System.ComponentModel.DataAnnotations;
using TrainineeAPI.Models;

namespace TrainineeAPI.DTOs;

public class FilterTraineeDto
{
    public string? Search { get; set; }
    public int? PageNumber {get; set;}
    public int? PageSize {get; set;}
    // TODO Status coversion to enum
    [EnumDataType(typeof(TraineeStatusEnumValues),ErrorMessage = "Invalid Status Value Specified")]
    public string? Status {get; set;}
    
}