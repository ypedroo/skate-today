using LanguageExt.Common;
using shouldISkateToday.Domain.Dtos;

namespace shouldISkateToday.Data.Repositories.Interfaces;

public interface IUserFavoritesRepository
{
    Task<Result<List<UserResponseDto>>> GetAllUsers();
    Task<Result<UserFavoritesDto>> GetUserFavoritesAsync(Guid userId);
    Task<Result<bool>> UpsertUserFavoritesAsync (Guid userId, string favorites);

}