# DECISIONS

Answer **yourself**, in your own words, in 3–5 sentences per question.
Refer to concrete files and classes from your own repo (for example `Features/Bookings/CreateBooking/...`).
An unanswered question gives 0 points.

## 1. A rule you moved
Pick one business rule (R1–R8). In which file did it live in the starter and in which file does it live now? Why there?

in starter regula de anulare r8 si acum r11 era pusa in BookingService prin schimbarea directa a proprietatii booking.Status in BookingStatus.Cancelled in noua arhitectura am mutat regula in entitatea Booking din Domain/Bookings/Booking.cs in metoda Cancel(DateTime nowUtc) 

## 2. Result instead of an exception
Why is `Result` a better choice than an exception for "the room is occupied"? What happens in your code when an `Error` is returned?

_(your answer here)_

## 3. Who knows about EF Core?
Which classes in your project know about EF Core and which do not? What do you gain and what do you pay with Repository + Unit of Work?

doar clasele din Infrastructure/Persistence adica RoomHubDbContext RoomConfiguration BookingConfiguration RoomRepository BookingRepository si Program.cs stiu de ef core nici un fisier din Domain sau Features nu foloseste ef core ci doar interfetele IRoomRepository IBookingRepository si IUnitOfWork am castigat separarea logicii de baza de date si testabilitatea mai usoara iar pretul este codul in plus pentru interfete si configurari

## 4. The flow of a request
Draw in text (ASCII arrows) the path of a `POST /api/rooms/{roomId}/bookings` request through your project, with the **real** names of files and classes, from the endpoint to the database.

_(your answer here)_

## 5. What you rejected from what the AI gave you
Give a concrete example of code or an idea proposed by the AI that you corrected or rejected, and say why.

initial ai a vrut sa puna SaveChangesAsync direct in RoomRepository si BookingRepository

## 6. If you had 30 more minutes
What would you improve and what risk did you leave in the project?

_(your answer here)_
