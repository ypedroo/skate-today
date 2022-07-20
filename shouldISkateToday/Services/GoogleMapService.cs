using LanguageExt.Common;
using Newtonsoft.Json;
using shouldISkateToday.Clients.RequestInterface;
using shouldISkateToday.Domain.Models;
using shouldISkateToday.Services.Interfaces;
using static System.Environment;

namespace shouldISkateToday.Services;

public class GoogleMapService : IGoogleMapService
{
    private readonly string? _apiKey;
    private readonly IGoogleMapsRequests _request;

    public GoogleMapService(IGoogleMapsRequests request, IConfiguration configuration)
    {
        _request = request;
        _apiKey= configuration.GetValue<string>("GOOGLE_MAPS_API_KEY");
    }
    public async Task<Result<SkateParks>> GetSkateParks(string spot)
    {
        try
        {
            var response = await _request.GetSkateParks(spot, _apiKey ?? string.Empty);
            var parks = JsonConvert.DeserializeObject<SkateParks>(response);
            return parks ?? new Result<SkateParks>();
        }
        catch (Exception exception)
        {
            return new Result<SkateParks>(exception);
        }
    }
}
