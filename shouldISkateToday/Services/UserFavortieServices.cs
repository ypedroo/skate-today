using LanguageExt.Common;
using shouldISkateToday.Data.Repositories.Interfaces;
using shouldISkateToday.Domain.Models;
using shouldISkateToday.Services.Interfaces;

namespace shouldISkateToday.Services;

public class UserFavoriteServices : IUserFavoritesService
{
    private readonly IUserFavoritesRepository _repository;

    public UserFavoriteServices(IUserFavoritesRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<UserFavorites>> GetUserFavoritesAsync(Guid userId, string favorites) =>
        await _repository.GetUserFavoritesAsync(userId, favorites);

    public async Task<Result<bool>> UpsertUserFavoritesAsync(Guid userId, string favorites) =>
        await _repository.UpsertUserFavoritesAsync(userId, favorites);
}