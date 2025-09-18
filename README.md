# Airport Routing API

A simple ASP.NET Core 8 Web API that provides airport connections and destinations. It fetches data from an external airport API, caches the response in memory, and exposes endpoints to query destinations from a given origin airport code.

---

## Table of Contents

- [Features](#features)
- [Prerequisites](#prerequisites)
- [Getting Started](#getting-started)
- [Configuration](#configuration)
- [API Endpoints](#api-endpoints)
- [Logging](#logging)
- [Unit Tests](#unit-tests)
- [Technologies](#technologies)
- [License](#license)

---

## Features

- Fetches airport and connection data from an external API.
- Maps API responses into DTOs using AutoMapper.
- In-memory caching of external API responses for performance.
- Global exception handling.
- Logging with Serilog.
- Unit tests with xUnit and Moq.

---

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- Visual Studio 2022 / VS Code
- Internet access to reach the external airport API

---

## Getting Started

1. **Clone the repository**

```bash
git clone https://github.com/yourusername/AirportRoutingApi.git
cd AirportRoutingApi
```

2. **Restore packages**

```bash
dotnet restore
```

3. **Run the API**

```bash
dotnet run --project AirportRoutingApi
```

4. **The API will start on https://localhost:44318 by default. Swagger UI is available at:**

https://localhost:44318/swagger/index.html

---

## Configuration

External API:
The base URL and required headers for the external airport API are configured in Program.cs using HttpClientFactory.

Serilog:
Logs are written to logs/log.txt in the project root.

JsonSerializerOptions:
Global configuration for case-insensitive deserialization.

---

## API Endpoints
Get Destinations by Origin
GET /api/airports/{origin}/destinations


Parameters:

Name	Type	Required	Description
origin	string	Yes	3-letter airport code (IATA)

Responses:

200 OK → Returns a list of destinations.

400 Bad Request → Invalid origin format.

404 Not Found → No destinations found for the given origin.

Example Response:

```
{
  "destinations": [
    { "name": "ACCRA", "code": "ACC", "currency": "USD", "countryCode": "GHA" },
    { "name": "Dubai DXB", "code": "DXB", "currency": "AED", "countryCode": "ARE" }
  ]
}
```

---

## Logging

All API calls and responses are logged using Serilog.

Logs are stored in logs/log.txt.

Example log entry:

2025-09-18 12:05:41 [INF] Airport API call succeeded, returned 26 airports

---

## Unit Tests

Unit tests are implemented using xUnit and Moq.

Test project structure:

```
/tests/AirportRoutingApi.Tests
│
├── Services
│   └── AirportServiceTests.cs
├── Controllers
│   └── AirportControllerTests.cs
└── TestData
    └── AirportApiMockData.cs
```

Run tests:
```
dotnet test
```

Example Unit Test:

```
[Fact]
public async Task GetDestinationsAsync_ReturnsMappedDestinations_WhenOriginExists()
{
    // Arrange
    var origin = "CPT";
    var apiResponse = AirportApiMockData.GetSampleAirportResponse(origin);

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
```

---

## Technologies

- ASP.NET Core 8

- C# 12

- AutoMapper

- Serilog

- xUnit & Moq

- HTTP Client Factory

---

## License

This project is licensed under the MIT License.