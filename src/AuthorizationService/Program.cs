using AuthService;
using AuthService.Configurations;
using AuthService.Services;
using AuthService.Services.Interfaces;
using Kernel.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration; // allows both to access and to set up the config
var environment = builder.Environment;
// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.Configure<JwtConfiguration>(configuration.GetSection("JwtConfiguration"));
builder.Services.Configure<GatewaySecret>(configuration.GetSection("GatewaySecret"));

builder.Services.AddTransient<IAuthService, AuthService.Services.AuthService>();
builder.Services.AddSingleton<LogsProducer>();

builder.Services.AddDbContext<AppDbContext>(x =>
{
    x.UseNpgsql(
        configuration.GetConnectionString("DefaultConnection")
    );
});

var app = builder.Build();

app.UseHttpLogging();

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
// }

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;

try
{
    var context = services.GetRequiredService<AppDbContext>();
}
catch (Exception e)
{
    // ignored
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(e, "An error occurred seeding the DB");
}

// app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
