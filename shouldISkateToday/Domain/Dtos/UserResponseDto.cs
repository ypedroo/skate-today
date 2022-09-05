namespace shouldISkateToday.Domain.Dtos;

public class UserResponseDto
{
    public Guid UserId { get; set; }
    public string? UserName { get; set; }
    public DateTime RefreshTokenExpires { get; set; }
}