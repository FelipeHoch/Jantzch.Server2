using Jantzch.Server2;
using Jantzch.Server2.Infraestructure;

var builder = WebApplication.CreateBuilder(args);

var startup = new Startup();

startup.ConfigureServices(builder.Services);

var app = builder.Build();

startup.Configure(app, builder.Environment);


app.Run();
