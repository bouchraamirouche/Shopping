using Microsoft.Extensions.Hosting;

using Shopping;
using Shopping.Models;





//var host = CreateHostBuilder(args).Build();

var app = Startup.InitializeApp(args);

var scope = app.Services.CreateScope();

var services = scope.ServiceProvider;

SeedData.Initialize(services);

app.Run();
