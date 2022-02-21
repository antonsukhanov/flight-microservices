using System.Text.Json;
using Confluent.Kafka;
using FlightMicroservices.Consumer.Models;
using FlightMicroservices.Consumer.Models.Contexts;
using FlightMicroservices.Consumer.Models.Dto;
using Microsoft.EntityFrameworkCore;

namespace FlightMicroservices.Consumer;

public class Worker : BackgroundService
{
    private const string TopicName = "flight-updates";
    private readonly IConsumer<Null, string> _consumer;
    private readonly ILogger<Worker> _logger;
    private readonly IServiceScopeFactory _scopeFactory;

    public Worker(ILogger<Worker> logger, IConsumer<Null, string> consumer, IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _consumer = consumer;
        _consumer.Subscribe(TopicName);
        _scopeFactory = scopeFactory ?? throw new ArgumentNullException(nameof(scopeFactory));
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Consumer started");

        using var scope = _scopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await db.Database.MigrateAsync(cancellationToken);

        while (!cancellationToken.IsCancellationRequested)
            try
            {
                var result = _consumer.Consume(cancellationToken);
                if (result.Topic != TopicName) throw new Exception($"Unknown message topic {result.Topic}");

                _logger.LogInformation("Processing message with offset '{MessageOffset}'", result.Offset);

                var report = JsonSerializer.Deserialize<FlightUpdateDto>(result.Message.Value);
                if (report is null) throw new Exception($"Message with offset '{result.Offset}' deserialized to null");

                var flight = await db.Flights.Where(f => f.FlightNumber == report.FlightNumber)
                    .FirstOrDefaultAsync(cancellationToken);

                if (flight is null)
                {
                    var newFlight = new Flight
                    {
                        Id = Guid.NewGuid(),
                        DepartureDate = report.DepartureDate,
                        FlightNumber = report.FlightNumber
                    };

                    await db.Flights.AddAsync(newFlight, cancellationToken);
                    await db.SaveChangesAsync(cancellationToken);

                    _logger.LogInformation(
                        "Created new flight with id '{FlightId}' (number '{FlightNumber}', departure date '{DepartureDate}')",
                        newFlight.Id, newFlight.FlightNumber, newFlight.DepartureDate);
                }
                else
                {
                    flight.DepartureDate = report.DepartureDate;
                    await db.SaveChangesAsync(cancellationToken);

                    _logger.LogInformation(
                        "Updated flight with id '{FlightId}' (number '{FlightNumber}', new departure date '{DepartureDate}')",
                        flight.Id, flight.FlightNumber, flight.DepartureDate);
                }

                _consumer.Commit(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured while processing messages");
            }
    }
}