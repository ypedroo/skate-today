﻿using Microsoft.AspNetCore.Mvc;
using shouldISkateToday.Extensions;
using shouldISkateToday.Services.Interfaces;

namespace shouldISkateToday.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SkateParkController : ControllerBase
{
    private readonly IGoogleMapService _service;

    public SkateParkController(IGoogleMapService service)
    {
        _service = service;
    }

    [HttpGet("skate-parks")]
    public async Task<IActionResult> GetGithubUser([FromQuery] string spot)
    {
        var result = await _service.GetSkateParks(spot);
        return result.ToOk(response => response);
    }
}