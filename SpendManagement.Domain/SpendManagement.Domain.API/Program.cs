using KafkaFlow;
using Crosscutting.Extensions;
using Crosscutting.Models;
using Crosscutting.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Host.ConfigureAppConfiguration((config) =>
{
    var currentDirectory = Directory.GetCurrentDirectory();
    config
        .SetBasePath(currentDirectory)
        .AddJsonFile($"{currentDirectory}/conf/appsettings.json");
}).ConfigureLogging(logging =>
{
    logging.ClearProviders();
    logging.AddFilter("Microsoft", LogLevel.Critical);
});

var applicationSettings = builder.Configuration.GetSection("Settings").Get<Settings>();

builder.Services.AddSingleton<ISettings>(applicationSettings ?? throw new Exception("Error while reading app settings."));

builder.Services
    .AddTracing()
    .AddHealthCheckers(applicationSettings)
    .AddKafka(applicationSettings.KafkaSettings)
    .AddRepositories()
    .AddServiceEventsProducer()
    .AddLoggingDependency()
    .AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.ShowKafkaDashboard();

app.UseHealthCheckers();

app.Run();
