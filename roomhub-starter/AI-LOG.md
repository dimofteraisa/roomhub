# AI-LOG

Write down, in order, what you asked the AI. It is not a punishment: a missing or false log lowers the level D grade.

| Time (approx.) | What you asked (briefly) | What you kept | What you changed or rejected |
|---|---|---|---|
| 9:15 | refactor layered RoomHub API into Vertical Slices (Features/), Rich Domain (Room, Booking), Result<T>, ICommandHandler, TimeProvider, and EF Core SQLite with Repository + IUnitOfWork (rules R1–R9). | Project folder structure (Common/CQRS, Domain, Features, Infrastructure), Result<T> / Error pattern, TimeProvider injection instead of DateTime.UtcNow, and EF Core configurations. | _(fill in)_ |
| 9:40 | implement new rules R10 (max 2h duration when room capacity > 10) and R11 (cancel allowed only >= 60 min before start using TimeProvider). | Domain checks inside Booking.Create and Booking.Cancel(nowUtc) returning Result<T> mapped to 400 and 409 ProblemDetails. | Rejected using DateTime.UtcNow in Booking.Cancel and passed timeProvider.GetUtcNow().UtcDateTime from CancelBookingHandler. |