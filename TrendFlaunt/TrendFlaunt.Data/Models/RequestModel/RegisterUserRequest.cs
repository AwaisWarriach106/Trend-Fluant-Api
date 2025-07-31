namespace TrendFlaunt.Data.Models.RequestModel;

public class RegisterUserRequest
{
    public string? UserId { get; set; }
    public string? FullName { get; set; }
    public string? PhoneNumber { get; set; }
    public string? UserType { get; set; }
    public string? Gender { get; set; }
    public string? Email { get; set; }
    public bool? EmailVerified { get; set; }
    public bool? PhoneNumberVerified { get; set; }
    public string Password { get; set; }

}
