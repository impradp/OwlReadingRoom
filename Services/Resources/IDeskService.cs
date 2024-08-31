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
    TableQuery<Desk> TableQuery { get; }
}
