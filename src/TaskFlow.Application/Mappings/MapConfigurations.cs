using Mapster;
using TaskFlow.Communication.Requests;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Application.Mappings;

public static class MapConfigurations
{
    public static void Configure()
    {
        TypeAdapterConfig<RequestRegisterUserDto, User>.NewConfig().Ignore(dest => dest.Password);
    }
}