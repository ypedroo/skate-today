using LanguageExt;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using shouldISkateToday.Data.Contexts;
using Testcontainers.MsSql;
using WebMotions.Fake.Authentication.JwtBearer;

namespace shouldISkateTodayTests;

public class SkateApiFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly MsSqlContainer _dbContainer = new MsSqlBuilder()
        .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
        .WithPassword("ype.1400")
        .WithPortBinding("1433")
        .WithCleanUp(true)
        .Build();

    public async Task InitializeAsync() => await _dbContainer.StartAsync();

    public new async Task DisposeAsync() => await _dbContainer.DisposeAsync();
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll(typeof(UserContext));
            services.RemoveAll(typeof(AuthenticationBuilder));
            services.AddAuthentication(FakeJwtBearerDefaults.AuthenticationScheme).AddFakeJwtBearer();

            services.AddSingleton(
                _ => new DbContextOptionsBuilder<UserContext>()
                    .UseSqlServer(_dbContainer.GetConnectionString())
                    .Options);
            services.AddDbContext<UserContext>();

            var serviceProvider = services.BuildServiceProvider();
            using var scope = serviceProvider.CreateScope();
            var scopedServices = scope.ServiceProvider;
            var context = scopedServices.GetRequiredService<UserContext>();
            context.Database.EnsureCreated();
            services.BuildServiceProvider();
        });
    }
}