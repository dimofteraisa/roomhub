using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using RoomHub.Api.Common;
using RoomHub.Api.Common.CQRS;
using RoomHub.Api.Domain.Common;
using RoomHub.Api.Features.Rooms.CreateRoom;

namespace RoomHub.Api.Features.Rooms.GetRoomById;

public static class GetRoomByIdEndpoint
{
    public static void Map(RouteGroupBuilder group) => group.MapGet("/{id:guid}", Handle)
        .WithName("GetRoomById");

    private static async Task<IResult> Handle(
        Guid id,
        IQueryHandler<GetRoomByIdQuery, Result<RoomResponse>> handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(new GetRoomByIdQuery(id), cancellationToken);

        if (result.IsFailure)
        {
            return result.Error.ToProblem();
        }

        return Results.Ok(result.Value);
    }
}
