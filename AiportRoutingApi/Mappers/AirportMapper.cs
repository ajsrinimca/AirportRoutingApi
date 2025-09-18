namespace AiportRoutingApi.Mappers;

public class AirportMapper : Profile
{
    public AirportMapper()
    {
        CreateMap<Connection, DestinationsDto>().ReverseMap();
    }
}