using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using shouldISkateToday.Extensions;
using shouldISkateToday.Services.Interfaces;

namespace shouldISkateToday.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class UserFavoriteController : ControllerBase
{
    private readonly IUserFavoritesService _service;

    public UserFavoriteController(IUserFavoritesService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetUserFavoritesAsync(Guid userId, string favorites)
    {
        var result = await _service.GetUserFavoritesAsync(userId, favorites);
        return result.ToOk(response => response);
    }

    [HttpPost]
    public async Task<IActionResult> UpsertUserFavoritesAsync(Guid userId, string favorites)
    {
        var result = await _service.UpsertUserFavoritesAsync(userId, favorites);
        return result.ToCreated(response => response);
    }
}