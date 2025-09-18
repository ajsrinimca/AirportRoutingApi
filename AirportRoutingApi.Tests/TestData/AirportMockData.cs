namespace AirportRoutingApi.Tests.TestData;

public static class AirportMockData
{
    public static AirportResponse GetAirportsResponse()
    {
        return new AirportResponse
        {
            Airports =
            [
                new Airport
                {
                    Code = "CPT",
                    Connections =
                    [
                        new Connection { Name = "ACCRA", Code = "ACC", Currency = "USD", CountryCode = "GHA" },
                        new Connection { Name = "Dubai DXB", Code = "DXB", Currency = "AED", CountryCode = "ARE" }
                    ]
                }
            ]
        };
    }

    public static AirportDestinationResponse GetAirportDestinationResponse()
    {
        return new AirportDestinationResponse
        {
            Destinations =
            [
                new DestinationsDto { Code = "ACC", Name = "ACCRA" },
                new DestinationsDto { Code = "DXB", Name = "Dubai DXB" }
            ]
        };
    }
}