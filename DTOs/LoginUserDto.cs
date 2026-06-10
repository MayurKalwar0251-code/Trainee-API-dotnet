using System.ComponentModel.DataAnnotations;
using TrainineeAPI.Models;

namespace TrainineeAPI.DTOs;

public class LoginUserDto
{
    [Required]
    public required string Username { get; set; }

    [Required]
    [EmailAddress]
    public required string Email { get; set; }

    [Required]
    public required string PasswordHash { get; set; }
}