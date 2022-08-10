using LanguageExt.Common;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using shouldISkateToday.Data.Contexts;
using shouldISkateToday.Data.Repositories.Interfaces;
using shouldISkateToday.Domain.Dtos;
using shouldISkateToday.Domain.Models;

namespace shouldISkateToday.Data.Repositories;

public class UserFavoritesRepository : IUserFavoritesRepository
{
    private readonly UserContext _context;

    public UserFavoritesRepository(UserContext context)
    {
        _context = context;
    }

    public async Task<Result<List<UserResponseDto>>> GetAllUsers()
    {
        try
        {
            var userFavorites = await _context.Users.ToListAsync();
            var userFavoritesDto = userFavorites.Select(x => new UserResponseDto
            {
                UserId = x.Id,
                UserName = x.UserName,
                RefreshTokenExpires = x.RefreshTokenExpires


            }).ToList();
            return new Result<List<UserResponseDto>>(userFavoritesDto);
        }
        catch (Exception exception)
        {
            return new Result<List<UserResponseDto>>(exception);
        }
    }

    public async Task<Result<UserFavoritesDto>> GetUserFavoritesAsync(Guid userId)
    {
        try
        {
            var userFavorites = await _context.UserFavorites.FirstOrDefaultAsync(x => x.UserId == userId);
            if (userFavorites == null)
            {
                var error = new KeyNotFoundException();
                return new Result<UserFavoritesDto>(error);
            }

            var user = new UserFavoritesDto
            {
                UserId = userFavorites.UserId,
                Favorites = JsonConvert.DeserializeObject<SkateParks>(userFavorites.Favorites)
            };
            
            return new Result<UserFavoritesDto>(user);
        }
        catch (Exception exception)
        {
            return new Result<UserFavoritesDto>(exception);
        }
    }

    public async Task<Result<bool>> UpsertUserFavoritesAsync(Guid userId, string favorites)
    {
        try
        {
            var userFavorites = await _context.UserFavorites.FirstOrDefaultAsync(x => x.UserId == userId);
            if (userFavorites == null)
            {
                var newUser = new UserFavorites
                {
                    Favorites = "",
                    UserId = userId
                };
                await _context.UserFavorites.AddAsync(newUser);
                await _context.SaveChangesAsync();
                return new Result<bool>(true);
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