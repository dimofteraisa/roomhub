using RoomHub.Api.Common.CQRS;
using RoomHub.Api.Domain.Bookings;
using RoomHub.Api.Domain.Common;

namespace RoomHub.Api.Features.Bookings.GetBookingById;

public sealed record GetBookingByIdQuery(Guid Id) : IQuery<Result<BookingResponse>>;

public sealed record BookingResponse(
    Guid Id,
    Guid RoomId,
    string BookedBy,
    DateTime StartUtc,
    DateTime EndUtc,
    int Attendees,
    BookingStatus Status);

public sealed class GetBookingByIdHandler(IBookingRepository bookingRepository)
    : IQueryHandler<GetBookingByIdQuery, Result<BookingResponse>>
{
    public async Task<Result<BookingResponse>> HandleAsync(GetBookingByIdQuery query, CancellationToken cancellationToken = default)
    {
        var booking = await bookingRepository.GetByIdAsync(query.Id, cancellationToken);
        if (booking is null)
        {
            return Result<BookingResponse>.Failure(BookingErrors.NotFound);
        }

        return Result<BookingResponse>.Success(new BookingResponse(
            booking.Id,
            booking.RoomId,
            booking.BookedBy,
            booking.StartUtc,
            booking.EndUtc,
            booking.Attendees,
            booking.Status));
    }
}
