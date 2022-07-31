using System.Data.Entity.Core;
using LanguageExt.Common;
using Microsoft.EntityFrameworkCore;
using shouldISkateToday.Data.Contexts;
using shouldISkateToday.Data.Repositories.Interfaces;
using shouldISkateToday.Domain.Models;

namespace shouldISkateToday.Data.Repositories;

public class UserFavoritesRepository : IUserFavoritesRepository
{
    private readonly UserContext _context;

    public UserFavoritesRepository(UserContext context)
    {
        _context = context;
    }

    public async Task<Result<UserFavorites>> GetUserFavoritesAsync(Guid userId, string favorites)
    {
        try
        {
            var userFavorites = await _context.UserFavorites.FirstOrDefaultAsync(x => x.UserId == userId);
            return new Result<UserFavorites>(userFavorites ?? new UserFavorites());
        }
        catch (Exception exception)
        {
            return new Result<UserFavorites>(exception);
        }
    }

    public async Task<Result<bool>> UpsertUserFavoritesAsync(Guid userId, string favorites)
    {
        try
        {
            var userFavorites = await _context.UserFavorites.FirstOrDefaultAsync(x => x.UserId == userId);
            if (userFavorites == null)
            {
                var userNotFoundError = new ObjectNotFoundException("User does not exists");
                return new Result<bool>(userNotFoundError);
            }

            userFavorites.Favorites = favorites;

            await _context.SaveChangesAsync();

            return new Result<bool>(true);
        }
        catch (Exception exception)
        {
            return new Result<bool>(exception);
        }
    }
}