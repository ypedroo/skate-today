using System.Dynamic;
namespace shouldISkateTodayTests.Helpers;

public static class IntegrationTestsHelper
{
    internal static dynamic GetAdminToken()
    {
        dynamic data = new ExpandoObject();
        data.sub = "dca45f95-aee7-435e-83d3-3ca5f5a1af0e";
        data.extension_UserRole = "Admin";
        return data;
    }
}