using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using RoomHub.Api.Common;
using RoomHub.Api.Common.CQRS;

namespace RoomHub.Api.Features.Bookings.CreateBooking;

public sealed record CreateBookingRequest(
    string? BookedBy,
    DateTime StartUtc,
    DateTime EndUtc,
    int Attendees);

public static class CreateBookingEndpoint
{
    public static void Map(RouteGroupBuilder group) => group.MapPost("/{roomId:guid}/bookings", Handle)
        .WithName("CreateBooking");

    private static async Task<IResult> Handle(
        Guid roomId,
        CreateBookingRequest request,
        ICommandHandler<CreateBookingCommand, BookingCreatedResponse> handler,
        CancellationToken cancellationToken)
    {
        var command = new CreateBookingCommand(
            roomId,
            request.BookedBy,
            request.StartUtc,
            request.EndUtc,
            request.Attendees);

        var result = await handler.HandleAsync(command, cancellationToken);

        if (result.IsFailure)
        {
            return result.Error.ToProblem();
        }

        return Results.Created($"/api/bookings/{result.Value.Id}", result.Value);
    }
}
