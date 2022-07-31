using LanguageExt.Common;
using shouldISkateToday.Domain.Models;

namespace shouldISkateToday.Data.Repositories.Interfaces;

public interface IUserFavoritesRepository
{
    Task<Result<UserFavorites>> GetUserFavoritesAsync(Guid userId, string favorites);
    Task<Result<bool>> UpsertUserFavoritesAsync (Guid userId, string favorites);

}