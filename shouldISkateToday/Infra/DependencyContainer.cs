﻿using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Refit;
using shouldISkateToday.Clients.RequestInterface;
using shouldISkateToday.Data.Contexts;
using shouldISkateToday.Data.Repositories;
using shouldISkateToday.Data.Repositories.Interfaces;
using shouldISkateToday.Services;
using shouldISkateToday.Services.Interfaces;

namespace shouldISkateToday.Infra;

public static class DependencyContainer
{
    public static IServiceCollection RegisterDependencies(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddTransient<IGoogleMapService, GoogleMapService>();
        services.AddTransient<IUserRepository, UserRepository>();
        services.AddTransient<IUserService, UserService>();
        services.AddTransient<IUserRepository, UserRepository>();
        services.AddTransient<IUserFavoritesService, UserFavoriteServices>();
        services.AddTransient<IUserFavoritesRepository, UserFavoritesRepository>();
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddMvc().AddJsonOptions(options => { options.JsonSerializerOptions.IncludeFields = true; });
        services.AddRefitClient<IGoogleMapsRequests>().ConfigureHttpClient(c =>
        {
            c.BaseAddress = new Uri(configuration.GetValue<string>("GOOGLE_MAPS_PLACE_URL") ?? string.Empty);
        });
        var connectionString = configuration.GetValue<string>("azure-db-connection");
        services.AddDbContext<UserContext>(options =>
            options.UseSqlServer(connectionString, sqlOptions =>
            {
                sqlOptions.CommandTimeout(180);
            })
        );
        services.AddFluentValidation(conf =>
        {
            conf.RegisterValidatorsFromAssemblyContaining<Program>();
            conf.AutomaticValidationEnabled = false;
        });
        return services;
    }
}