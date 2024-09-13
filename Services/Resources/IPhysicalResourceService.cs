using OwlReadingRoom.Models;
using OwlReadingRoom.Proxy;
using OwlReadingRoom.ViewModels;

namespace OwlReadingRoom.Services.Resources;

public interface IPhysicalResourceService
{
    /// <summary>
    /// Fetches a list of rooms with their desk availability information.
    /// </summary>
    /// <returns>
    /// A list of RoomListViewModel objects containing room details and desk availability.
    /// </returns>
    /// <remarks>
    /// This method performs the following steps:
    /// 1. Retrieves room information and desk counts using a join operation.
    /// 2. Fetches the count of unavailable desks for each room.
    /// 3. Calculates the number of available desks for each room.
    /// 4. Constructs and returns a list of RoomListViewModel objects with the compiled information.
    /// </remarks>
    List<RoomListViewModel> fetchRooms();

    /// <summary>
    /// Retrieves a list of desk information view models for a specific room.
    /// </summary>
    /// <param name="roomId">The ID of the room for which to retrieve desk information.</param>
    /// <returns>
    /// A list of <see cref="DeskInfoViewModel"/> objects containing information about each desk 
    /// in the specified room, including its status, color coding, and reservation details (if any).
    /// </returns>
    /// <remarks>
    /// The method first retrieves the list of desks for the specified room and then fetches the 
    /// corresponding booking information. It iterates through each desk and creates a view model 
    /// that includes the desk's status and reservation message, using the first reservation if 
    /// multiple reservations exist. If no reservation exists, default values are used.
    /// </remarks>
    List<DeskInfoViewModel> GetDeskInfoPerRoom(int roomId);

    /// <summary>
    /// Updates a room's information and adds new desks if specified.
    /// </summary>
    /// <param name="roomViewModel">The view model containing the room's current information.</param>
    /// <param name="roomName">The new name for the room.</param>
    /// <param name="numberOfDesks">The number of new desks to add to the room.</param>
    /// <param name="deskInitial">The initial string to use for naming new desks.</param>
    /// <remarks>
    /// This method performs the following operations within a transaction:
    /// 1. Adds new desks to the room if numberOfDesks is greater than 0.
    /// 2. Updates the room's name if it has changed.
    /// If an exception occurs during the process, the transaction is rolled back.
    /// </remarks>
    void UpdateRoom(RoomListViewModel roomViewModel, string roomName, int numberOfDesks, string deskInitial);


    void AddRooms(RoomType roomType, int? numberOfRooms, string roomInitials = "RM");
}
