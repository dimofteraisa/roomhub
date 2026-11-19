using RoomHub.Api.Common.CQRS;
using RoomHub.Api.Domain.Common;
using RoomHub.Api.Domain.Rooms;

namespace RoomHub.Api.Features.Rooms.CreateRoom;

public sealed record CreateRoomCommand(string? Name, int Capacity) : ICommand<RoomResponse>;

public sealed record RoomResponse(Guid Id, string Name, int Capacity);

public sealed class CreateRoomHandler(IRoomRepository roomRepository, IUnitOfWork unitOfWork)
    : ICommandHandler<CreateRoomCommand, RoomResponse>
{
    public async Task<Result<RoomResponse>> HandleAsync(CreateRoomCommand command, CancellationToken cancellationToken = default)
    {
        var result = Room.Create(command.Name, command.Capacity);
        if (result.IsFailure)
        {
            return Result<RoomResponse>.Failure(result.Error);
        }

        await roomRepository.AddAsync(result.Value, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<RoomResponse>.Success(new RoomResponse(result.Value.Id, result.Value.Name, result.Value.Capacity));
    }
}
