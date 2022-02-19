using FlightMicroservices.WebApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace FlightMicroservices.WebApi.Controllers;

[ApiController]
[Produces("application/json")]
[Route("api/v1/flight-updates")]
public class FlightUpdatesController : ControllerBase
{
    private readonly ILogger<FlightUpdatesController> _logger;

    public FlightUpdatesController(ILogger<FlightUpdatesController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Processes new flight update report.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async ValueTask<IActionResult> ProcessFlightUpdate([FromBody] FlightUpdateDto flightUpdateDto)
    {
        _logger.LogInformation(
            "Received update for flight '{FlightNumber}' (new departure date '{DepartureDate}') from host '{Host}'",
            flightUpdateDto.FlightNumber, flightUpdateDto.DepartureDate,
            HttpContext.Connection.RemoteIpAddress?.ToString() ?? "UNKNOWN_HOST");

        return NoContent();
    }
}