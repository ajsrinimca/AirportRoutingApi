namespace AiportRoutingApi.Services;

public class AirportApiClient(
    HttpClient httpClient,
    ILogger<AirportApiClient> logger,
    IMemoryCache cache) : IAirportApiClient
{
    private const string CacheKey = "AirportsCache";

    public async Task<AirportResponse> GetAirportsAsync(CancellationToken cancellationToken = default)
    {
        if (cache.TryGetValue(CacheKey, out AirportResponse cachedResponse))
        {
            logger.LogInformation("Returning airports from memory cache");

            return cachedResponse;
        }

        logger.LogInformation("Calling external airport API at {BaseAddress}", httpClient.BaseAddress);

        var response = await httpClient.GetAsync("Airport/OriginsWithConnections/en", cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            logger.LogError("Airport API call failed with status {StatusCode}", response.StatusCode);
            throw new HttpRequestException($"Airport API call failed with status code {response.StatusCode}");
        }

        await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
        var jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var result = await JsonSerializer.DeserializeAsync<AirportResponse>(stream, jsonOptions, cancellationToken);

        if (result == null)
        {
            throw new InvalidOperationException("Airport API returned null.");
        }

        cache.Set(CacheKey, result, TimeSpan.FromMinutes(10));

        logger.LogInformation("Airport API call succeeded, returned {Count} airports", result.Airports.Count);

        return result;
    }
}