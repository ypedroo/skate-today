using System.ComponentModel.DataAnnotations;

namespace shouldISkateToday.Domain.Models;

public class UserFavorites
{
    [Key]
    public Guid UserId { get; set; }
    public string Favorites { get; set; }
}