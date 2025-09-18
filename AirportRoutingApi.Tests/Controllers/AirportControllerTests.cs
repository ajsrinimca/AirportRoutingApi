namespace AirportRoutingApi.Tests.Controllers;

public class AirportControllerTests
{
    private readonly Mock<IAirportService> _mockService;
    private readonly Mock<ILogger<AirportController>> _mockLogger;
    private readonly AirportController _controller;

    public AirportControllerTests()
    {
        _mockService = new Mock<IAirportService>();
        _mockLogger = new Mock<ILogger<AirportController>>();
        _controller = new AirportController(_mockService.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task GetDestinations_ReturnsOk_WithDestinations()
    {
        // Arrange
        var origin = "CPT";
        var destinations = AirportMockData.GetAirportDestinationResponse();
        _mockService.Setup(s => s.GetDestinationsAsync(origin))
                    .ReturnsAsync(destinations);

        // Act
        var result = await _controller.GetDestinations(origin);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<AirportDestinationResponse>(okResult.Value);
        Assert.Equal(2, returnValue.Destinations.Count);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("AB")]
    [InlineData("ABCD")]
    [InlineData("123")]
    public async Task GetDestinations_ReturnsBadRequest_WhenOriginInvalid(string origin)
    {
        // Act
        var result = await _controller.GetDestinations(origin);

        // Assert
        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Contains("Origin must be exactly 3 alphabetic characters", badRequest.Value.ToString());
    }

    [Fact]
    public async Task GetDestinations_ReturnsNotFound_WhenNoDestinations()
    {
        // Arrange
        var origin = "XYZ";
        _mockService.Setup(s => s.GetDestinationsAsync(origin))
                    .ReturnsAsync((AirportDestinationResponse?)null);

        // Act
        var result = await _controller.GetDestinations(origin);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Contains(origin.ToUpperInvariant(), notFoundResult.Value.ToString());
    }
}