using RoomHub.Api.Domain.Common;

namespace RoomHub.Api.Domain.Bookings;

public sealed class Booking
{
    private Booking() { }

    public Guid Id { get; private set; }
    public Guid RoomId { get; private set; }
    public string BookedBy { get; private set; } = string.Empty;
    public DateTime StartUtc { get; private set; }
    public DateTime EndUtc { get; private set; }
    public int Attendees { get; private set; }
    public BookingStatus Status { get; private set; }

    public static Result<Booking> Create(
        Guid roomId,
        string? bookedBy,
        DateTime startUtc,
        DateTime endUtc,
        int attendees,
        int roomCapacity,
        TimeProvider timeProvider)
    {
        return Create(roomId, bookedBy, startUtc, endUtc, attendees, roomCapacity, timeProvider.GetUtcNow().UtcDateTime);
    }

    public static Result<Booking> Create(
        Guid roomId,
        string? bookedBy,
        DateTime startUtc,
        DateTime endUtc,
        int attendees,
        int roomCapacity,
        DateTime nowUtc)
    {
        if (string.IsNullOrWhiteSpace(bookedBy))
        {
            return Result<Booking>.Failure(BookingErrors.InvalidBookedBy);
        }

        var start = startUtc.Kind == DateTimeKind.Unspecified ? DateTime.SpecifyKind(startUtc, DateTimeKind.Utc) : startUtc.ToUniversalTime();
        var end = endUtc.Kind == DateTimeKind.Unspecified ? DateTime.SpecifyKind(endUtc, DateTimeKind.Utc) : endUtc.ToUniversalTime();

        if (end <= start)
        {
            return Result<Booking>.Failure(BookingErrors.InvalidRange);
        }

        if (attendees <= 0)
        {
            return Result<Booking>.Failure(BookingErrors.InvalidAttendees);
        }

        if (attendees > roomCapacity)
        {
            return Result<Booking>.Failure(BookingErrors.AttendeesExceedCapacity);
        }

        if (start < nowUtc)
        {
            return Result<Booking>.Failure(BookingErrors.PastStartUtc);
        }

        if (end - start > TimeSpan.FromHours(4))
        {
            return Result<Booking>.Failure(BookingErrors.MaxDurationExceeded);
        }

        return Result<Booking>.Success(new Booking
        {
            Id = Guid.NewGuid(),
            RoomId = roomId,
            BookedBy = bookedBy.Trim(),
            StartUtc = start,
            EndUtc = end,
            Attendees = attendees,
            Status = BookingStatus.Active
        });
    }

    public Result<Unit> Cancel()
    {
        if (Status == BookingStatus.Cancelled)
        {
            return Result<Unit>.Failure(BookingErrors.AlreadyCancelled);
        }

        Status = BookingStatus.Cancelled;
        return Result<Unit>.Success(Unit.Value);
    }
}
