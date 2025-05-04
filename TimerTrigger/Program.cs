using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TimerTrigger.Services;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

// Application Insights isn't enabled by default. See https://aka.ms/AAt8mw4.
// builder.Services
//     .AddApplicationInsightsTelemetryWorkerService()
//     .ConfigureFunctionsApplicationInsights();
//var connectionString = builder.Configuration.GetValue<string>("AzureStorage:ConnectionString");
//var queueName = builder.Configuration.GetValue<string>("AzureStorage:QueueName");
var queueName = Environment.GetEnvironmentVariable("QueueName");
var connectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
builder.Services.AddSingleton<IQueueService>(sp => new QueueService(connectionString, queueName));
builder.Build().Run();
