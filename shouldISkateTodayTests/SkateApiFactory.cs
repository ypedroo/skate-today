using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using shouldISkateToday.Data.Contexts;
using WebMotions.Fake.Authentication.JwtBearer;

namespace shouldISkateTodayTests;

public class SkateApiFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly MsSqlTestcontainer _dbContainer = new TestcontainersBuilder<MsSqlTestcontainer>()
        .WithDatabase(new MsSqlTestcontainerConfiguration
        {
            Password = "ype.1400",
        })
        .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
        .WithCleanUp(true)
        .Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {

            services.RemoveAll(typeof(DbContext));
            services.RemoveAll(typeof(AuthenticationBuilder));
            services.AddAuthentication(FakeJwtBearerDefaults.AuthenticationScheme).AddFakeJwtBearer();
            services.AddSingleton(
                _ => new DbContextOptionsBuilder<UserContext>()
                    .UseSqlServer(_dbContainer.ConnectionString)
                    .Options);
            var serviceProvider = services.BuildServiceProvider();

            using var scope = serviceProvider.CreateScope();
            var scopedServices = scope.ServiceProvider;
            var context = scopedServices.GetRequiredService<UserContext>();
            context.Database.EnsureCreated();
            services.BuildServiceProvider();
        });
        
        
    }

    public async Task InitializeAsync() => await _dbContainer.StartAsync();

    public new async Task DisposeAsync() => await _dbContainer.StopAsync();
}