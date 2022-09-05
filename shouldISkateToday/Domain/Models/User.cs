using System.ComponentModel.DataAnnotations.Schema;

namespace shouldISkateToday.Domain.Models;

[Table("Users")]
public class User
{
    public Guid Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public byte[]? PasswordHash { get; set; }
    public byte[]? PasswordSalt { get; set; }
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime RefreshTokenExpires { get; set; }
    public DateTime RefreshTokenCreated { get; set; }
}