using RoomHub.Api.Common.CQRS;
using RoomHub.Api.Domain.Common;
using RoomHub.Api.Domain.Rooms;
using RoomHub.Api.Features.Rooms.CreateRoom;

namespace RoomHub.Api.Features.Rooms.GetRoomById;

public sealed record GetRoomByIdQuery(Guid Id) : IQuery<Result<RoomResponse>>;

public sealed class GetRoomByIdHandler(IRoomRepository roomRepository)
    : IQueryHandler<GetRoomByIdQuery, Result<RoomResponse>>
{
    public async Task<Result<RoomResponse>> HandleAsync(GetRoomByIdQuery query, CancellationToken cancellationToken = default)
    {
        var room = await roomRepository.GetByIdAsync(query.Id, cancellationToken);
        if (room is null)
        {
            return Result<RoomResponse>.Failure(RoomErrors.NotFound);
        }

        return Result<RoomResponse>.Success(new RoomResponse(room.Id, room.Name, room.Capacity));
    }
}
