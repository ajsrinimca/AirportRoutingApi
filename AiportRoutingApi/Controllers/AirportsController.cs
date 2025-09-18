using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace AiportRoutingApi.Controllers;

[ApiController]
[Route("api/airports")]
public class AirportsController(
    IAirportService airportService,
    ILogger<AirportsController> logger) : ControllerBase
{
    [HttpGet("{origin}/destinations")]
    [ProducesResponseType(typeof(AirportDestinationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetDestinations([FromRoute] string origin)
    {
        logger.LogInformation("Request received: {Method} {Path} origin={Origin}",
            Request.Method, Request.Path, origin);

        if (string.IsNullOrWhiteSpace(origin) ||
            origin.Length != 3 ||
            !Regex.IsMatch(origin, "^[A-Za-z]{3}$"))
        {
            logger.LogWarning("Invalid origin provided: {Origin}", origin);
            return BadRequest("Origin must be exactly 3 alphabetic characters.");
        }

        var destinations = await airportService.GetDestinationsAsync(origin.ToUpperInvariant());

        if (destinations?.Destinations == null || destinations.Destinations.Count == 0)
        {
            logger.LogInformation("No destinations found for origin={Origin}", origin);
            return NotFound($"No destinations found for {origin.ToUpperInvariant()}.");
        }

        var json = JsonSerializer.Serialize(destinations, new JsonSerializerOptions
        {
            WriteIndented = false,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        logger.LogInformation("Returning full response for {Origin}: {ResponseJson}", origin, json);

        return Ok(destinations);
    }
}