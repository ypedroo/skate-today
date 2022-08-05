using System.Dynamic;
using System.Net;
using System.Web;
using FluentAssertions;
using FluentAssertions.Execution;


namespace shouldISkateTodayTests.IntegrationTests
{
    public class SkateParksControllerTest : IClassFixture<SkateApiFactory>
    {
        private readonly SkateApiFactory _factory;

        public SkateParksControllerTest(SkateApiFactory factory)
        {
            _factory = factory;
        }

        private static dynamic GetAdminToken()
        {
            dynamic data = new ExpandoObject();
            data.sub = "dca45f95-aee7-435e-83d3-3ca5f5a1af0e";
            data.extension_UserRole = "Admin";
            return data;
        }

        [Fact]
        public async Task Get_Returns_All_SkateParks()
        {
            var data = GetAdminToken();
            using var client = _factory.CreateDefaultClient();
            client.SetFakeBearerToken((object) data);

            var query = HttpUtility.ParseQueryString(string.Empty);
            query["spot"] = "Skate parks in Brazil";

            var queryString = query.ToString();

            var response = await client.GetAsync($"api/SkatePark/skate-parks?{queryString}");


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

            var query = HttpUtility.ParseQueryString(string.Empty);
            query["spot"] = "Skate parks in Brazil";

            var queryString = query.ToString();

            var response = await client.GetAsync($"api/SkatePark/skate-parks?{queryString}");


            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }
    }
}