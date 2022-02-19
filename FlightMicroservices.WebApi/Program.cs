using System.Reflection;
using Confluent.Kafka;
using Confluent.Kafka.DependencyInjection;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Flight Updates API",
        Description = "API consumes flight update reports",
        Contact = new OpenApiContact
        {
            Name = "Anton Sukhanov",
            Email = string.Empty,
        }
    });

    // Set the comments path for the Swagger JSON and UI.
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});
builder.Services.AddKafkaClient(new ProducerConfig
{
    BootstrapServers = "localhost:9092"
});

// Configure middlewares.

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();

app.Run();