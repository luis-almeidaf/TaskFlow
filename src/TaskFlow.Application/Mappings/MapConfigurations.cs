using Mapster;
using TaskFlow.Application.Features.Boards.Columns.Cards.Commands.UpdateCardCommand;
using TaskFlow.Application.Features.Boards.Queries.GetBoardByIdQuery;
using TaskFlow.Application.Features.Boards.Queries.GetBoardByIdQuery.Responses;
using TaskFlow.Application.Features.Users.Commands.RegisterUserCommand;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Application.Mappings;

public static class MapConfigurations
{
    public static void Configure()
    {
        TypeAdapterConfig<RegisterUserCommand, User>.NewConfig().Ignore(dest => dest.Password);

        TypeAdapterConfig<UpdateCardCommand, Card>.NewConfig()
            .Ignore(dest => dest.CreatedBy)
            .Ignore(dest => dest.CreatedById);
    }
}