using OwlReadingRoom.Models;
using SQLite;

namespace OwlReadingRoom.Services.Resources;

public interface IRoomService
{
    /// <summary>
    /// Adds rooms to the database based on the specified parameters.
    /// </summary>
    /// <param name="roomType">The type of room to be added.</param>
    /// <param name="numberOfRooms">The number of rooms to add. If null, no rooms will be added.</param>
    /// <param name="roomInitials">
    /// The initials to use for generating room identifiers. Default value is "RM".
    /// </param>
    /// <remarks>
    /// This method adds the specified number of rooms to the database. Each room is
    /// assigned a unique identifier based on the provided room initials.
    ///  
    /// Room naming convention:
    /// - Room names are generated in the format: {roomInitials}-{number}
    /// - If rooms with the given initials already exist, numbering starts from the last existing number + 1
    /// - If no rooms with the given initials exist, numbering starts from 1
    /// 
    /// If <paramref name="numberOfRooms"/> is null or less than or equal to 0, no rooms will be added.
    /// </remarks>
    void AddRooms(RoomType roomType, int? numberOfRooms, string roomInitials = "RM");

    /// <summary>
    /// Retrieves a room by its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the room to retrieve.</param>
    /// <returns>
    /// A Room object representing the room with the specified ID.
    /// </returns>
    /// <remarks>
    /// This method directly calls the GetItem method of the room repository.
    /// If no room is found with the given ID, a default value of null is returned.
    /// </remarks>
    Room GetRoomById(int id);

    /// <summary>
    /// Saves changes to a room in the database.
    /// </summary>
    /// <param name="room">The Room object containing the updated information to be saved.</param>
    /// <returns>
    /// An integer value, typically representing the number of rows affected in the database.
    /// This could be 1 if the save was successful, or 0 if no changes were made.
    /// </returns>
    /// <remarks>
    /// This method calls the SaveItem method of the room repository to persist the changes.
    /// </remarks>
    int Save(Room room);    
    TableQuery<Room> TableQuery { get; }
}
