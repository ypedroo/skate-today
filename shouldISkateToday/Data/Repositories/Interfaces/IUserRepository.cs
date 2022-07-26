using LanguageExt.Common;
using shouldISkateToday.Domain.Models;

namespace shouldISkateToday.Data.Repositories.Interfaces;

public interface IUserRepository
{
    Task<Result<User>> AddUser(User user);
    Task<Result<User>> GetUser(string userId);
    Task<Result<string>> UpdateUserRefreshTokenUpdateUserRefreshToken(User validUser);
}