namespace AirportRoutingApi.Models;

public class Connection
{
    public string Name { get; set; }
    public string Code { get; set; }
    public string Currency { get; set; }
    public string CountryCode { get; set; }
    public bool RestrictedOnDeparture { get; set; }
    public bool RestrictedOnDestination { get; set; }
}