using OwlReadingRoom.Models;
using SQLite;

namespace OwlReadingRoom.Services.Resources;

public interface IDeskService
{
    /// <summary>
    /// Adds a specified number of desks to a room.
    /// </summary>
    /// <param name="roomId">The ID of the room to add desks to.</param>
    /// <param name="numberOfDesks">The number of desks to add. Must be non-negative.</param>
    /// <param name="deskInitials">The initial string to use for naming the desks. Defaults to "DSK".</param>
    /// <remarks>
    /// This method performs the following steps:
    /// 1. Validates that the number of desks is non-negative.
    /// 2. Determines the starting number for new desk names.
    /// 3. Creates new Desk objects with incrementing names.
    /// 4. Inserts all new desks into the repository.
    /// If numberOfDesks is negative, it displays an error message and doesn't add any desks.
    /// </remarks>
    void AddDesks(int roomId, int? numberOfDesks, string deskInitials = "DSK");

    /// <summary>
    /// Retrieves a list of desks located in a specific room.
    /// </summary>
    /// <param name="roomId">The ID of the room for which to retrieve the desks.</param>
    /// <returns>
    /// A list of <see cref="Desk"/> objects that belong to the specified room.
    /// </returns>
    /// <remarks>
    /// The method queries the desk repository to find all desks associated with the given room ID 
    /// and returns them as a list.
    /// </remarks>
    List<Desk> GetDesksOfRoom(int roomId);
    TableQuery<Desk> TableQuery { get; }

    /// <summary>
    /// Retrieves a desk by its ID.
    /// </summary>
    /// <param name="deskId">The ID of the desk to retrieve.</param>
    /// <returns>The desk with the specified ID.</returns>
    Desk GetDesk(int? deskId);

    List<Desk> AddDesks(List<Desk> desks);
}
