using LanguageExt.Common;
using shouldISkateToday.Data.Repositories.Interfaces;
using shouldISkateToday.Domain.Dtos;
using shouldISkateToday.Services.Interfaces;

namespace shouldISkateToday.Services;

public class UserFavoriteServices : IUserFavoritesService
{
    private readonly IUserFavoritesRepository _repository;

    public UserFavoriteServices(IUserFavoritesRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<List<UserResponseDto>>> GetAllUsers() =>  await _repository.GetAllUsers();

    public async Task<Result<UserFavoritesDto>> GetUserFavoritesAsync(Guid userId) =>
        await _repository.GetUserFavoritesAsync(userId);

    public async Task<Result<bool>> UpsertUserFavoritesAsync(Guid userId, string? favorites) =>
        await _repository.UpsertUserFavoritesAsync(userId, favorites);
}