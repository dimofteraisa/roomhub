using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using RoomHub.Api.Common;
using RoomHub.Api.Common.CQRS;

namespace RoomHub.Api.Features.Rooms.CreateRoom;

public sealed record CreateRoomRequest(string? Name, int Capacity);

public static class CreateRoomEndpoint
{
    public static void Map(RouteGroupBuilder group) => group.MapPost("/", Handle)
        .WithName("CreateRoom");

    private static async Task<IResult> Handle(
        CreateRoomRequest request,
        ICommandHandler<CreateRoomCommand, RoomResponse> handler,
        CancellationToken cancellationToken)
    {
        var command = new CreateRoomCommand(request.Name, request.Capacity);
        var result = await handler.HandleAsync(command, cancellationToken);

        if (result.IsFailure)
        {
            return result.Error.ToProblem();
        }

        return Results.Created($"/api/rooms/{result.Value.Id}", result.Value);
    }
}
