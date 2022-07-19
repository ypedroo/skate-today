using Microsoft.AspNetCore.Mvc;
using Refit;
using shouldISkateToday.Extensions;
using shouldISkateToday.Services.Interfaces;

namespace shouldISkateToday.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GithubController : ControllerBase
{
    private readonly IGithubService _service;

    public GithubController(IGithubService service)
    {
        _service = service;
    }

    [HttpGet("user/{user}")]
    public async Task<IActionResult> GetGithubUser(string user)
    {
        var result = await _service.GetUserById(user);
        return result.ToOk(response => response);
    }
}