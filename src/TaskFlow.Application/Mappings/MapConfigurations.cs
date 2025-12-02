using Mapster;
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
        TypeAdapterConfig<Board, GetBoardByIdResponse>.NewConfig().Map(dest => dest.CreatedBy, src =>
            new CreatorBoardResponse()
            {
                Id = src.CreatedBy.Id,
                Name = src.CreatedBy.Name
            });
        TypeAdapterConfig<User, UserResponse>.NewConfig();
        TypeAdapterConfig<Column, ColumnResponse>.NewConfig();
    }
}