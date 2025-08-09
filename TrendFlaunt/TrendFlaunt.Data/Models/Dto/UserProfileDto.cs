namespace TrendFlaunt.Data.Models.Dto;

public class UserProfileDto
{
    public Guid? Id { get; set; }
    public string? UserId { get; set; }
    public string? FullName { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Address { get; set; }
    public string? ProfileImageUrl { get; set; }
    public string? Gender { get; set; }
    public string? Email { get; set; }
}
