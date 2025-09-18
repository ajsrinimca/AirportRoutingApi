namespace AirportRoutingApi.Tests.Services;

public class AirportServiceTests
{
    private readonly Mock<IAirportApiClient> _mockApiClient;
    private readonly IMapper _mapper;
    private readonly AirportService _airportService;

    public AirportServiceTests()
    {
        _mockApiClient = new Mock<IAirportApiClient>();

        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Connection, DestinationsDto>();
        });

        _mapper = config.CreateMapper();

        _airportService = new AirportService(_mockApiClient.Object, _mapper);
    }

    [Fact]
    public async Task GetDestinationsAsync_ReturnsMappedDestinations_WhenOriginExists()
    {
        // Arrange
        var origin = "CPT";
        var apiResponse = AirportMockData.GetAirportsResponse();
        _mockApiClient.Setup(x => x.GetAirportsAsync(It.IsAny<CancellationToken>()))
                      .ReturnsAsync(apiResponse);

        // Act
        var result = await _airportService.GetDestinationsAsync(origin);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Destinations.Count);
        Assert.Contains(result.Destinations, d => d.Code == "ACC");
        Assert.Contains(result.Destinations, d => d.Code == "DXB");
    }

    [Fact]
    public async Task GetDestinationsAsync_ReturnsEmptyList_WhenOriginDoesNotExist()
    {
        // Arrange
        _mockApiClient.Setup(x => x.GetAirportsAsync(It.IsAny<CancellationToken>()))
                      .ReturnsAsync(new AirportResponse { Airports = new List<Airport>() });

        // Act
        var result = await _airportService.GetDestinationsAsync("XYZ");

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result.Destinations);
    }

    [Fact]
    public async Task GetDestinationsAsync_ReturnsNull_WhenApiReturnsNull()
    {
        // Arrange
        _mockApiClient.Setup(x => x.GetAirportsAsync(It.IsAny<CancellationToken>()))
                      .ReturnsAsync((AirportResponse?)null);

        // Act
        var result = await _airportService.GetDestinationsAsync("CPT");

        // Assert
        Assert.Null(result);
    }
}