using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using shouldISkateToday.Data.Contexts;

namespace shouldISkateTodayTests;

public class SkateApiFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlTestcontainer _dbContainer = new TestcontainersBuilder<PostgreSqlTestcontainer>()
        .WithDatabase(new PostgreSqlTestcontainerConfiguration
        {
            Database = "postgres",
            Username = "ypedro",
            Password = "ype.1400",
        })
        .Build();


    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.BuildServiceProvider();
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "Test";
                options.DefaultChallengeScheme = "Test";
            });
            services.RemoveAll(typeof(DbContext));
            services.AddDbContext<UserContext>(options => options.UseNpgsql(_dbContainer.ConnectionString));
            // var serviceProvider = services.BuildServiceProvider();

            // using var scope = serviceProvider.CreateScope();
            // var scopedServices = scope.ServiceProvider;
            // var context = scopedServices.GetRequiredService<DbContext>();
            // context.Database.EnsureCreated();
        });
    }

    public async Task InitializeAsync() => await _dbContainer.StartAsync();

    public new async Task DisposeAsync() => await _dbContainer.StopAsync();
}