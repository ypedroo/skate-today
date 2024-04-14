using System.Security.Cryptography;
using System.Text;

namespace shouldISkateToday.Extensions;

public static class JwtExtensions
{
    
    public static RefreshToken GenerateRefreshToken()
    {
        var refreshToken = new RefreshToken
        {
            Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
            Expires = DateTime.UtcNow.AddDays(7),
            Created = DateTime.UtcNow
        };
        return refreshToken;
    }
    public static bool VerifyPasswordHash(string requestPassword, byte[]? userPasswordHash, byte[]? userPasswordSalt)
    {
        using var hmac = new HMACSHA256(userPasswordSalt);
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(requestPassword));
        return computedHash.SequenceEqual(userPasswordHash);
    }
    
    public static void CreatePasswordHash(string password, out byte[]? passwordHash, out byte[]? passwordSalt)
    {
        using var hmac = new HMACSHA256();
        passwordSalt = hmac.Key;
        passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
    }
}