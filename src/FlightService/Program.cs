using FlightService;
using FlightService.Profiles;
using Kernel.Extensions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration; // allows both to access and to set up the config
var environment = builder.Environment;
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSingleton<LogsProducer>();

builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(x => { x.AddProfile<FlightProfile>(); });

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
app.UseSwagger();
app.UseSwaggerUI();
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