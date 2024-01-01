var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.shouldISkateToday>("shouldISkateToday");
builder.Build().Run();
