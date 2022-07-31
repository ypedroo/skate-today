using LanguageExt.Common;
using shouldISkateToday.Domain.Models;
using shouldISkateToday.Services.Interfaces;

namespace shouldISkateToday.Services;

// TODO - update services an tests the endpoints
public class UserFavoriteServices : IUserFavoritesService
{
    public Task<Result<UserFavorites>> GetUserFavoritesAsync(Guid userId, string favorites) => throw new NotImplementedException();

    public Task<Result<bool>> UpsertUserFavoritesAsync(Guid userId, string favorites) => throw new NotImplementedException();
}