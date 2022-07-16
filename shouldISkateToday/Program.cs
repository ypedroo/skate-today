using shouldISkateToday.Infra;
using shouldISkateToday.Services;
using shouldISkateToday.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);
builder.Services.RegisterDependencies(builder.Configuration);

var app = builder.Build();
app.UseRouting();
app.MapGet("/github/{user}", async (string user,IGithubService service) => await service.GetUserById(user));

app.Run();