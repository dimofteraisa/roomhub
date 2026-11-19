using Microsoft.EntityFrameworkCore;
using RoomHub.Api.Domain.Rooms;

namespace RoomHub.Api.Infrastructure.Persistence.Repositories;

public sealed class RoomRepository(RoomHubDbContext dbContext) : IRoomRepository
{
    public async Task<Room?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await dbContext.Rooms.FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
    }

    public async Task AddAsync(Room room, CancellationToken cancellationToken = default)
    {
        await dbContext.Rooms.AddAsync(room, cancellationToken);
    }
}
