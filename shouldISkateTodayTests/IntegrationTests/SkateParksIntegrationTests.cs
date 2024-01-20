// using System.Net;
// using System.Web;
// using FluentAssertions;
// using FluentAssertions.Execution;
// using LanguageExt;
// using shouldISkateTodayTests.Helpers;
//
//
// namespace shouldISkateTodayTests.IntegrationTests
// {
//     public class SkateParksIntegrationTests : IClassFixture<SkateApiFactory>
//     {
//         private readonly SkateApiFactory _factory;
//
//         public SkateParksIntegrationTests(SkateApiFactory factory)
//         {
//             _factory = factory;
//         }
//
//         [Fact]
//         public async Task Get_Returns_All_SkateParks()
//         {
//             var data = IntegrationTestsHelper.GetAdminToken();
//             using var client = _factory.CreateDefaultClient();
//             client.SetFakeBearerToken((object) data);
//
//             var query = HttpUtility.ParseQueryString(string.Empty);
//             query["spot"] = "Skate parks in Brazil";
//
//             var queryString = query.ToString();
//
//             var response = await client.GetAsync($"api/SkatePark/skate-parks?{queryString}");
//
//             // Assert
//             using (new AssertionScope())
//             {
//                 response.StatusCode.Should().Be(HttpStatusCode.OK);
//                 response.Content.Should().BeOfType<StreamContent>();
//                 response.Content.Should().NotBeNull();
//             }
//         }
//
//         [Fact]
//         public async Task Should_Give_Unauthorized_If_User_Does_Not_Have_Token()
//         {
//             using var client = _factory.CreateDefaultClient();
//
//             var query = HttpUtility.ParseQueryString(string.Empty);
//             query["spot"] = "Skate parks in Brazil";
//
//             var queryString = query.ToString();
//
//             var response = await client.GetAsync($"api/SkatePark/skate-parks?{queryString}");
//
//
//             // Assert
//             response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
//         }
//     }
// }