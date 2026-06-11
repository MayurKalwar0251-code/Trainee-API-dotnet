namespace TrainineeAPI.Models;

public enum MentorStatusEnumValues {Active, Inactive};
public class Mentor
{
    public long Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required string Expertise { get; set; }
    public required string Status { get; set; }
    public DateOnly CreatedDate { get; set; }
    public DateOnly UpdatedDate { get; set; }
}