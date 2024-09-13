using OwlReadingRoom.Models;
using OwlReadingRoom.Services.Repository;
using OwlReadingRoom.Utils;
using SQLite;

namespace OwlReadingRoom.Services.Resources;

public class DeskService : IDeskService
{
    private readonly IRepository<Desk> _deskRepository;

    public DeskService(IRepository<Desk> deskRepository)
    {
        _deskRepository = deskRepository;
    }

    public TableQuery<Desk> TableQuery => _deskRepository.Table;

    [Obsolete("Please use AddDesks(int) instead.")]
    public void AddDesks(int roomId, int? numberOfDesks, string deskInitials = "DSK")
    {
        if (numberOfDesks == null || numberOfDesks < 0)
        {
            CustomAlert.ShowAlert("Error", "The number of desks cannot be negative. Please enter a non-negative integer.", "OK");
            return;
        }

        int initialDeskNumber = GetLastDeskNumber(roomId, deskInitials);
        List<Desk> deskList = new List<Desk>();

        for (int i = 0; i < numberOfDesks; i++)
        {
            int deskNumber = initialDeskNumber + i;
            deskList.Add(new Desk()
            {
                Name = $"{deskInitials}-{deskNumber.ToString("D2")}",
                RoomId = roomId
            });
        }
        _deskRepository.InsertAll(deskList);
    }

    public List<Desk> AddDesks(List<Desk> desks)
    {
        _deskRepository.InsertAll(desks);
        return desks;
    }

    public List<Desk> GetDesksOfRoom(int roomId)
    {
        return _deskRepository.Table.Where(d => d.RoomId == roomId).ToList();
    }

    private int GetLastDeskNumber(int? roomId, string deskInitials)
    {
        var lastDesk = _deskRepository.Table.Where(desk => desk.RoomId == roomId && desk.Name.StartsWith(deskInitials))
            .OrderByDescending(desk => desk.Id)
            .FirstOrDefault();

        if (lastDesk != null)
        {
            return int.Parse(lastDesk.Name.Substring(deskInitials.Length + 1)) + 1;
        }

        return 1;
    }
}
