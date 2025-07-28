namespace TrendFlaunt.Data.Settings;

public class JwtTokenConfiguration
{
    public string TokenIssuer { get; set; }
    public string TokenAudience { get; set; }
    public int TokenDefaultExpiryMinutes { get; set; }
    public string TokenSecret { get; set; }
}
