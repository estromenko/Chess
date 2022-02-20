using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace Chess.Services;

public class AuthService
{
    private string Salt { get; set; }
    private int Expire { get; set; }

    public AuthService()
    {
        Salt = Environment.GetEnvironmentVariable("JWT_KEY")!;
        Expire = int.Parse(Environment.GetEnvironmentVariable("JWT_EXPIRE")!);
    }
    public string GenerateToken(string username)
    {
        byte[] symmetricKey = Convert.FromBase64String(Salt);
        var tokenHandler = new JwtSecurityTokenHandler();

        var now = DateTime.UtcNow;
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, username)
            }),

            Expires = now.AddMinutes(Convert.ToInt32(Expire)),

            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(symmetricKey),
                SecurityAlgorithms.HmacSha256Signature
            ),
        };

        var stoken = tokenHandler.CreateToken(tokenDescriptor);
        var token = tokenHandler.WriteToken(stoken);

        return token;
    }
}