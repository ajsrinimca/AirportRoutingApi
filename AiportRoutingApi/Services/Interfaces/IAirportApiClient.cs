namespace AiportRoutingApi.Services.Interfaces;

public interface IAirportApiClient
{
    Task<AirportResponse> GetAirportsAsync(CancellationToken cancellationToken = default);
}