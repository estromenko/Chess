using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Konscious.Security.Cryptography;

namespace Chess.Services;

public class AuthService
{
    private string JWTKey { get; set; }
    private int JWTExpire { get; set; }
    private string JWTIssuer { get; set; }

    private string ArgonSalt { get; set; }

    public AuthService()
    {
        JWTKey = Environment.GetEnvironmentVariable("JWT_KEY")!;
        JWTExpire = int.Parse(Environment.GetEnvironmentVariable("JWT_EXPIRE")!);
        JWTIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER")!;

        ArgonSalt = Environment.GetEnvironmentVariable("ARGON_SALT")!;
    }

    public string GenerateToken(string username)
    {
        ClaimsIdentity claims = new ClaimsIdentity(
            new[]{
                new Claim(ClaimsIdentity.DefaultNameClaimType, username),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, "user"),
            },
            "Token",
            ClaimsIdentity.DefaultNameClaimType,
            ClaimsIdentity.DefaultRoleClaimType
        );

        JwtSecurityToken jwt = new JwtSecurityToken(
            issuer: JWTIssuer,
            audience: JWTIssuer,
            notBefore: DateTime.UtcNow,
            claims: claims.Claims,
            expires: DateTime.UtcNow.AddMinutes(JWTExpire),
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JWTKey)),
                SecurityAlgorithms.HmacSha256
            )
        );

        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }

    public string HashPassword(string password)
    {
        Argon2id argon2 = new Argon2id(Encoding.UTF8.GetBytes(password));

        argon2.Salt = Encoding.UTF8.GetBytes(ArgonSalt);
        argon2.DegreeOfParallelism = 1;
        argon2.Iterations = 1;
        argon2.MemorySize = 1024 * 1024;

        byte[] bytes = argon2.GetBytes(64);
        return Convert.ToBase64String(bytes);
    }
}
