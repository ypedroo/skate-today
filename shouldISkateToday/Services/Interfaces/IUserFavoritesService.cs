using LanguageExt.Common;
using shouldISkateToday.Domain.Models;

namespace shouldISkateToday.Services.Interfaces;

public interface IUserFavoritesService
{
    Task<Result<UserFavorites>> GetUserFavoritesAsync(Guid userId, string favorites);
    Task<Result<bool>> UpsertUserFavoritesAsync (Guid userId, string favorites);
}