using System.ComponentModel.DataAnnotations;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace FlightMicroservices.Consumer.Models.Dto;

public class FlightUpdateDto
{
    [Required]
    public DateTime DepartureDate { get; init; }

    [Required]
    public string FlightNumber { get; init; } = null!;
}