namespace RoomHub.Api.Domain.Bookings;

public interface IBookingRepository
{
    Task<Booking?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task AddAsync(Booking booking, CancellationToken cancellationToken = default);
    Task<bool> HasOverlapAsync(Guid roomId, DateTime startUtc, DateTime endUtc, CancellationToken cancellationToken = default);
    Task<int> CountActiveBookingsForUserOnDateAsync(string bookedBy, DateTime date, CancellationToken cancellationToken = default);
}
