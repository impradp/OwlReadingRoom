using OwlReadingRoom.Models;
using OwlReadingRoom.Services.Repository;
using OwlReadingRoom.Utils;
using SQLite;

namespace OwlReadingRoom.Services.Resources;

public class RoomService : IRoomService
{

    private readonly IRepository<Room> _roomRepository;

    public RoomService(IRepository<Room> roomRepository)
    {
        _roomRepository = roomRepository;
    }

    public TableQuery<Room> TableQuery => _roomRepository.Table;

    public List<Room> AddRooms(RoomType roomType, int? numberOfRooms, string roomInitials = "RM")
    {
        int initialRoomNumber = GetLastRoomNumber(roomInitials);
        List<Room> roomList = new List<Room>();

        for (int i = 0; i < numberOfRooms; i++)
        {
            int roomNumber = initialRoomNumber + i;
            roomList.Add(new Room() { 
                Name = $"{roomInitials}-{roomNumber.ToString("D2")}",
                RoomType = roomType
            });
        }
        
        _roomRepository.InsertAll(roomList);
        return roomList;

    }

    private int GetLastRoomNumber(string roomInitials)
    {
        Room lastRoom = _roomRepository.Table.Where(room => room.Name.StartsWith(roomInitials))
            .OrderByDescending(room => room.Name)
            .FirstOrDefault();

        if (lastRoom != null)
        {
            return int.Parse(lastRoom.Name.Substring(roomInitials.Length + 1)) + 1;
        }

        return 1;
    }

    public Room GetRoomById(int id)
    {
        return _roomRepository.GetItem(id);
    }

    public int Save(Room room)
    {
        return _roomRepository.SaveItem(room);
    }
}
