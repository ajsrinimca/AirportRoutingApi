namespace AirportRoutingApi.Mappers;

public class AirportMapper : Profile
{
    public AirportMapper()
    {
        CreateMap<Connection, DestinationsDto>().ReverseMap();
    }
}