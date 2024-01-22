var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.shouldISkateToday>("should-i-skate-today");
builder.Build().Run();
