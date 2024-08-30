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
}
