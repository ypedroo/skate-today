using shouldISkateToday.Domain.Models;

namespace shouldISkateToday.Clients;
public class StormglassHttpClient
{
    private static readonly HttpClient Client = new();
    
    public async Task<SkateParks> GetSkateSpots(string url, int x, int y)
    {
        var result = await Client.GetAsync(url);
        if (result.StatusCode != System.Net.HttpStatusCode.OK)
            throw new HttpRequestException($"{result.StatusCode}-{result.RequestMessage}");

        var spots = await result.Content.ReadAsStringAsync();
        return null;
    }
}