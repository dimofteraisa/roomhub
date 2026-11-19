using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using RoomHub.Api.Common;
using RoomHub.Api.Common.CQRS;
using RoomHub.Api.Domain.Common;

namespace RoomHub.Api.Features.Bookings.GetBookingById;

public static class GetBookingByIdEndpoint
{
    public static void Map(RouteGroupBuilder group) => group.MapGet("/{id:guid}", Handle)
        .WithName("GetBookingById");

    private static async Task<IResult> Handle(
        Guid id,
        IQueryHandler<GetBookingByIdQuery, Result<BookingResponse>> handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(new GetBookingByIdQuery(id), cancellationToken);

        if (result.IsFailure)
        {
            return result.Error.ToProblem();
        }

        return Results.Ok(result.Value);
    }
}
