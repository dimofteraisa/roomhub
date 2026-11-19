using RoomHub.Api.Common.CQRS;
using RoomHub.Api.Domain.Bookings;
using RoomHub.Api.Domain.Common;
using RoomHub.Api.Domain.Rooms;

namespace RoomHub.Api.Features.Bookings.CreateBooking;

public sealed record CreateBookingCommand(
    Guid RoomId,
    string? BookedBy,
    DateTime StartUtc,
    DateTime EndUtc,
    int Attendees) : ICommand<BookingCreatedResponse>;

public sealed record BookingCreatedResponse(Guid Id, Guid RoomId, BookingStatus Status);

public sealed class CreateBookingHandler(
    IRoomRepository roomRepository,
    IBookingRepository bookingRepository,
    IUnitOfWork unitOfWork,
    TimeProvider timeProvider)
    : ICommandHandler<CreateBookingCommand, BookingCreatedResponse>
{
    public async Task<Result<BookingCreatedResponse>> HandleAsync(CreateBookingCommand command, CancellationToken cancellationToken = default)
    {
        // Check if room exists first (404 before any other validation)
        var room = await roomRepository.GetByIdAsync(command.RoomId, cancellationToken);
        if (room is null)
        {
            return Result<BookingCreatedResponse>.Failure(RoomErrors.NotFound);
        }

        // Domain validation: R1-R6
        var nowUtc = timeProvider.GetUtcNow().UtcDateTime;
        var bookingResult = Booking.Create(
            command.RoomId,
            command.BookedBy,
            command.StartUtc,
            command.EndUtc,
            command.Attendees,
            room.Capacity,
            nowUtc);

        if (bookingResult.IsFailure)
        {
            return Result<BookingCreatedResponse>.Failure(bookingResult.Error);
        }

        // R9: Participant cannot have more than 2 active bookings per day cumulated across all rooms
        var activeCount = await bookingRepository.CountActiveBookingsForUserOnDateAsync(
            command.BookedBy!,
            command.StartUtc,
            cancellationToken);

        if (activeCount >= 2)
        {
            return Result<BookingCreatedResponse>.Failure(BookingErrors.MaxDailyBookingsExceeded);
        }

        // R7: Overlap check for the room
        var hasOverlap = await bookingRepository.HasOverlapAsync(
            command.RoomId,
            command.StartUtc,
            command.EndUtc,
            cancellationToken);

        if (hasOverlap)
        {
            return Result<BookingCreatedResponse>.Failure(BookingErrors.RoomOccupied);
        }

        var booking = bookingResult.Value;
        await bookingRepository.AddAsync(booking, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<BookingCreatedResponse>.Success(
            new BookingCreatedResponse(booking.Id, booking.RoomId, booking.Status));
    }
}
