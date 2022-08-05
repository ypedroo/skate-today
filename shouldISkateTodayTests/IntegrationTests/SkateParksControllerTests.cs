using System.Net;
using System.Net.Http.Headers;
using System.Text;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using shouldISkateToday.Data.Contexts;
using shouldISkateToday.Domain.Dtos;

namespace shouldISkateTodayTests.IntegrationTests;

public class SkateParksControllerTest : IClassFixture<SkateApiFactory>
{
    private readonly SkateApiFactory _factory;

    public SkateParksControllerTest(SkateApiFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Get_Returns_All_SkateParks()
    {
        var client = _factory.CreateDefaultClient();
        var userToLogin = new UserDto
        {
            Username = "monkeyDLuffy",
            Password = "string"
        };

        await client.PostAsync("api/auth/register",
            new StringContent(JsonConvert.SerializeObject(userToLogin), Encoding.UTF8, "application/json"));

        var login = await client.PostAsync("api/auth/login",
            new StringContent(JsonConvert.SerializeObject(userToLogin), Encoding.UTF8, "application/json"));
        var token = await login.Content.ReadAsStringAsync();

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await client.GetAsync("api/skatepark/skate-parks?spot=skateparksInRussia");


        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}