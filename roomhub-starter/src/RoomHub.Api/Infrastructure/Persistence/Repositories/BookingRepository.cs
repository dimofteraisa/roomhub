using Microsoft.EntityFrameworkCore;
using RoomHub.Api.Domain.Bookings;

namespace RoomHub.Api.Infrastructure.Persistence.Repositories;

public sealed class BookingRepository(RoomHubDbContext dbContext) : IBookingRepository
{
    public async Task<Booking?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await dbContext.Bookings.FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
    }

    public async Task AddAsync(Booking booking, CancellationToken cancellationToken = default)
    {
        await dbContext.Bookings.AddAsync(booking, cancellationToken);
    }

    public async Task<bool> HasOverlapAsync(Guid roomId, DateTime startUtc, DateTime endUtc, CancellationToken cancellationToken = default)
    {
        var start = startUtc.Kind == DateTimeKind.Unspecified ? DateTime.SpecifyKind(startUtc, DateTimeKind.Utc) : startUtc.ToUniversalTime();
        var end = endUtc.Kind == DateTimeKind.Unspecified ? DateTime.SpecifyKind(endUtc, DateTimeKind.Utc) : endUtc.ToUniversalTime();

        return await dbContext.Bookings
            .AnyAsync(b => b.RoomId == roomId
                           && b.Status == BookingStatus.Active
                           && b.StartUtc < end
                           && start < b.EndUtc,
                cancellationToken);
    }

    public async Task<int> CountActiveBookingsForUserOnDateAsync(string bookedBy, DateTime date, CancellationToken cancellationToken = default)
    {
        var normalizedUser = bookedBy.Trim().ToLower();
        var utcDate = date.Kind == DateTimeKind.Unspecified ? DateTime.SpecifyKind(date, DateTimeKind.Utc) : date.ToUniversalTime();
        var dayStart = utcDate.Date;
        var dayEnd = dayStart.AddDays(1);

        return await dbContext.Bookings
            .Where(b => b.Status == BookingStatus.Active
                        && b.StartUtc >= dayStart
                        && b.StartUtc < dayEnd
                        && b.BookedBy.ToLower() == normalizedUser)
            .CountAsync(cancellationToken);
    }
}
