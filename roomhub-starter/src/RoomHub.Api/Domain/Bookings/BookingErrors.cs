using RoomHub.Api.Domain.Common;

namespace RoomHub.Api.Domain.Bookings;

public static class BookingErrors
{
    public static readonly Error InvalidBookedBy = Error.Validation("InvalidBookedBy", "BookedBy is required.");
    public static readonly Error InvalidRange = Error.Validation("InvalidRange", "The end must be after the start.");
    public static readonly Error InvalidAttendees = Error.Validation("InvalidAttendees", "Attendees must be positive.");
    public static readonly Error AttendeesExceedCapacity = Error.Validation("AttendeesExceedCapacity", "The room does not have enough seats.");
    public static readonly Error PastStartUtc = Error.Validation("PastStartUtc", "A booking cannot start in the past.");
    public static readonly Error MaxDurationExceeded = Error.Validation("MaxDurationExceeded", "The booking exceeds the maximum allowed duration.");
    public static readonly Error RoomOccupied = Error.Conflict("RoomOccupied", "The room is occupied in this interval.");
    public static readonly Error NotFound = Error.NotFound("BookingNotFound", "The booking does not exist.");
    public static readonly Error AlreadyCancelled = Error.Conflict("BookingAlreadyCancelled", "The booking is already cancelled.");
    public static readonly Error CancellationWindowPassed = Error.Conflict("CancellationWindowPassed", "A booking cannot be cancelled less than 60 minutes before its start.");
    public static readonly Error MaxDailyBookingsExceeded = Error.Conflict("MaxDailyBookingsExceeded", "A participant cannot have more than 2 active bookings on the same day.");
}
