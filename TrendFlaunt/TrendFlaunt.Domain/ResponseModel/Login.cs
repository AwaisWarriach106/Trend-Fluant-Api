namespace TrendFlaunt.Domain.ResponseModel;

public record LoginUserModel
{
    public string Email { get; set; }
    public string Password { get; set; }
}
