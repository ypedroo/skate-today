using shouldISkateToday.Domain.Models;

namespace shouldISkateToday.Domain.Dtos;

public class UserFavoritesDto
{
    public Guid UserId { get; set; }
    public SkateParks? Favorites { get; set; }
}