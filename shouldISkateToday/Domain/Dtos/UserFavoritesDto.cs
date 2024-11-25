using shouldISkateToday.Domain.Models;

namespace shouldISkateToday.Domain.Dtos;

public class UserFavoritesDto
{
    public Guid UserId { get; set; }
    public string? Favorites { get; set; }
}