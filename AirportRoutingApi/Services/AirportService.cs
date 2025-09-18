namespace AirportRoutingApi.Services;

public class AirportService(
    IAirportApiClient airportApiClient,
    IMapper mapper) : IAirportService
{
    public async Task<AirportDestinationResponse?> GetDestinationsAsync(string origin)
    {
        var airports = await airportApiClient.GetAirportsAsync();

        if (airports == null)
        {
            return null;
        }

        var connections = airports.Airports
            .FirstOrDefault(x => x.Code.Equals(origin, StringComparison.OrdinalIgnoreCase))
            ?.Connections ?? [];

        var desinations = mapper.Map<List<DestinationsDto>>(connections);

        return new AirportDestinationResponse
        {
            Destinations = desinations
        };
    }
}