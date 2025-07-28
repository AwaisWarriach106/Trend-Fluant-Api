using TrendFlaunt_Api;

var builder = WebApplication.CreateBuilder(args);
Startup.Configuration(builder.Services, builder.Configuration);
var app = builder.Build();
Startup.Configure(app);
app.Run();
