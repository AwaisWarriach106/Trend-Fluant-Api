using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using TrendFlaunt.Data.Settings;
using TrendFlaunt.Domain.ResponseModel;

namespace TrendFlaunt.Domain.Authentication;

public class TokenFactory : ITokenFactory
{
    private readonly JwtTokenConfiguration _jwtConfig;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IConfiguration _configuration;
    public TokenFactory(IOptions<JwtTokenConfiguration> jwtConfig, UserManager<IdentityUser> userManager, IConfiguration configuration)
    {
        _jwtConfig = jwtConfig.Value;
        _userManager = userManager;
        _configuration = configuration;
    }
    public UserSession Generate(string userId, string email, string userRoles)
    {
        if (_jwtConfig.TokenDefaultExpiryMinutes <= 0)
            _jwtConfig.TokenDefaultExpiryMinutes = 30;

        DateTime issuedAt = DateTime.UtcNow;
        DateTime expires = DateTime.UtcNow.AddMinutes(_jwtConfig.TokenDefaultExpiryMinutes);
        byte[] key = Convert.FromBase64String(_jwtConfig.TokenSecret);
        SymmetricSecurityKey securityKey = new SymmetricSecurityKey(key);
        IDictionary<string, object> lstClaims = new Dictionary<string, object>()
        {
            { ClaimTypes.NameIdentifier, userId},
            { ClaimTypes.Email, email},
            { "userRole", userRoles},
        };

        SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor
        {
            Issuer = _jwtConfig.TokenIssuer,
            Audience = _jwtConfig.TokenAudience,
            IssuedAt = issuedAt,
            Expires = expires,
            Claims = lstClaims,
            SigningCredentials = new SigningCredentials(securityKey,
            SecurityAlgorithms.HmacSha256Signature)
        };

        JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
        JwtSecurityToken token = handler.CreateJwtSecurityToken(descriptor);
        string tkn = handler.WriteToken(token);

        return new UserSession
        {
            Token = tkn
        };
    }

    public string CreateHash(string inputString)
    {
        SHA512 sha512 = SHA512.Create();
        byte[] bytes = Encoding.UTF8.GetBytes(inputString);
        byte[] hash = sha512.ComputeHash(bytes);
        return GetStringFromHash(hash);
    }
    private static string GetStringFromHash(byte[] hash)
    {
        StringBuilder result = new StringBuilder();
        for (int i = 0; i < hash.Length; i++)
        {
            result.Append(hash[i].ToString("X2"));
        }
        return result.ToString();
    }

    public bool Compare(string input, string hash)
    {
        return CreateHash(input).Equals(hash);
    }
}
