using System.Text.Json;
using Confluent.Kafka;
using FlightMicroservices.WebApi.Models;
using Microsoft.AspNetCore.Mvc;

// ReSharper disable BuiltInTypeReferenceStyle

namespace FlightMicroservices.WebApi.Controllers;

[ApiController]
[Produces("application/json")]
[Route("api/v1/flight-updates")]
public class FlightUpdatesController : ControllerBase
{
    private const string TopicName = "flight-updates";
    private readonly ILogger<FlightUpdatesController> _logger;
    private readonly IProducer<Null, String> _producer;

    public FlightUpdatesController(ILogger<FlightUpdatesController> logger, IProducer<Null, String> producer)
    {
        _logger = logger;
        _producer = producer;
    }

    /// <summary>
    /// Processes new flight update report
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async ValueTask<IActionResult> ProcessFlightUpdate([FromBody] FlightUpdateDto flightUpdateDto)
    {
        _logger.LogInformation(
            "[{TraceIdentifier}] Received update for flight '{FlightNumber}' (new departure date '{DepartureDate}') from host '{Host}'",
            HttpContext.TraceIdentifier, flightUpdateDto.FlightNumber, flightUpdateDto.DepartureDate,
            HttpContext.Connection.RemoteIpAddress?.ToString() ?? "UNKNOWN_HOST");

        var message = new Message<Null, String>
        {
            Value = JsonSerializer.Serialize(flightUpdateDto)
        };
        
        var result = await _producer.ProduceAsync(TopicName, message);
        
        _logger.LogInformation("[{TraceIdentifier}] Published flight update report to topic '{TopicName}' with offset '{MessageOffset}'",
            HttpContext.TraceIdentifier, TopicName, result.Offset.Value);
        
        return NoContent();
    }
}