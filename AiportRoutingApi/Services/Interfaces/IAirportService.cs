namespace AiportRoutingApi.Services.Interfaces;

public interface IAirportService
{
    Task<AirportDestinationResponse?> GetDestinationsAsync(string origin);
}