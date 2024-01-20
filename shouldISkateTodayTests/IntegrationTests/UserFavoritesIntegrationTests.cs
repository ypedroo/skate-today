using System.Net;
using FluentAssertions;
using FluentAssertions.Execution;
using shouldISkateToday.Data.Contexts;
using shouldISkateTodayTests.Helpers;

namespace shouldISkateTodayTests.IntegrationTests;

public class UserFavoritesIntegrationTests : IClassFixture<SkateApiFactory>
{
    private readonly SkateApiFactory _factory;

    public UserFavoritesIntegrationTests(SkateApiFactory factory) => _factory = factory;

    [Fact]
    public async Task Should_return_all_users()
    {
        var data = IntegrationTestsHelper.GetAdminToken();
        using var client = _factory.CreateDefaultClient();
        client.SetFakeBearerToken((object) data);
        

        var response = await client.GetAsync("api/UserFavorite/GetUsers");


        // Assert
        using (new AssertionScope())
        {
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Content.Should().BeOfType<StreamContent>();
            response.Content.Should().NotBeNull();
        }
    }

    [Fact]
    public async Task Should_Give_Unauthorized_If_User_Does_Not_Have_Token()
    {
        using var client = _factory.CreateDefaultClient();
        

        var response = await client.GetAsync("api/UserFavorite/GetUsers");


        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
    
    [Fact]
    public async Task Should_return_not_found_users_favorites()
    {
        var data = IntegrationTestsHelper.GetAdminToken();
        using var client = _factory.CreateDefaultClient();
        client.SetFakeBearerToken((object) data);
        

        var response = await client.GetAsync("api/UserFavorite?userId=79df7468-3b33-4dc1-a23e-8d9e2386f437");


        // Assert
        using (new AssertionScope())
        {
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            response.Content.Should().BeOfType<StreamContent>();
            response.Content.Should().NotBeNull();
        }
    }

    [Fact]
    public async Task Should_Give_Unauthorized_If_User_Does_Not_Have_Token_for_user_favorites()
    {
        using var client = _factory.CreateDefaultClient();
        

        var response = await client.GetAsync("api/UserFavorite?userId=79df7468-3b33-4dc1-a23e-8d9e2386f437");


        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}