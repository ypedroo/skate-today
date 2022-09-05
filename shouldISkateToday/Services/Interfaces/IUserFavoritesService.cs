using LanguageExt.Common;
using shouldISkateToday.Domain.Dtos;

namespace shouldISkateToday.Services.Interfaces;

public interface IUserFavoritesService
{
    Task<Result<List<UserResponseDto>>> GetAllUsers();
    Task<Result<UserFavoritesDto>> GetUserFavoritesAsync(Guid userId);
    Task<Result<bool>> UpsertUserFavoritesAsync (Guid userId, string? favorites);
}