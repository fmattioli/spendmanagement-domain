using KafkaFlow;
using Crosscutting.Extensions;
using Crosscutting.Models;
using Crosscutting.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

var enviroment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

builder.Configuration
    .AddJsonFile("conf/appsettings.json", false, reloadOnChange: true)
    .AddJsonFile($"conf/appsettings.{enviroment}.json", true, reloadOnChange: true)
    .AddEnvironmentVariables();

var applicationSettings = builder.Configuration.GetApplicationSettings(builder.Environment);

builder.Logging
    .ClearProviders()
    .AddFilter("Microsoft", LogLevel.Warning)
    .AddFilter("Microsoft", LogLevel.Critical);

builder.Services.AddSingleton<ISettings>(applicationSettings ?? throw new Exception("Error while reading app settings."));

builder.Services
    .AddTracing(applicationSettings.TracingSettings)
    .AddHealthCheckers(applicationSettings)
    .AddKafka(applicationSettings.KafkaSettings)
    .AddUnitOfWork(applicationSettings.SqlSettings.ConnectionString)
    .AddRepositories()
    .AddServiceEventsProducer()
    .AddLoggingDependency();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger()
   .UseSwaggerUI();

app.UseHttpsRedirection();

app.ShowKafkaDashboard();

app.UseHealthCheckers();

app.MapGet("/", () =>
"Hello! I'm working. My work is only read commands and producing domain events...\n" +
"If you wanna, you can see my health on /health");

var kafkaBus = app.Services.CreateKafkaBus();
await kafkaBus.StartAsync();
app.Run();
