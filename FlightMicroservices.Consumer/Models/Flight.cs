using System.ComponentModel.DataAnnotations;

namespace FlightMicroservices.Consumer.Models;

public class Flight
{
    [Required] public Guid Id { get; init; }

    [Required] public DateTime DepartureDate { get; set; }

    [Required] public string FlightNumber { get; set; } = null!;
}