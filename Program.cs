using Jantzch.Server2;

var builder = WebApplication.CreateBuilder(args);

var startup = new Startup();

var configuration = builder.Configuration;  

startup.ConfigureServices(builder.Services, configuration);

var app = builder.Build();

startup.Configure(app, builder.Environment);

app.Run();
