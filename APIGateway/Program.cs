using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
builder.Services.AddOcelot();

var app = builder.Build();

app.MapGet("/", () => "API Gateway is running");

try
{
    await app.UseOcelot();
}
catch (Exception ex)
{
    Console.WriteLine(" Ocelot failed to start:");
    Console.WriteLine(ex.Message);
    Console.WriteLine(ex.StackTrace);
}

app.Run();
