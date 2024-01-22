using LanguageExt.Common;
using Newtonsoft.Json;
using shouldISkateToday.Clients.RequestInterface;
using shouldISkateToday.Domain.Models;
using shouldISkateToday.Services.Interfaces;

namespace shouldISkateToday.Services;

public class GoogleMapService : IGoogleMapService
{
    private readonly string? _apiKey;
    private readonly IGoogleMapsRequests _request;

    public GoogleMapService(IGoogleMapsRequests request, IConfiguration configuration)
    {
        _request = request;
        _apiKey = configuration.GetValue<string>("google-maps-api-key") ?? string.Empty;
    }

    public async Task<Result<SkateParks>> GetSkateParks(string? spot)
    {
        try
        {
            if (string.IsNullOrEmpty(spot))
            {
                var error = new BadHttpRequestException("Spot is required");
                return new Result<SkateParks>(error);
            }
            var response = await _request.GetSkateParks(spot, _apiKey ?? string.Empty);
            var parks = JsonConvert.DeserializeObject<SkateParks>(response.Content ?? string.Empty);
            return parks ?? new Result<SkateParks>(new SkateParks());
        }
        catch (Exception exception)
        {
            return new Result<SkateParks>(exception);
        }
    }
}