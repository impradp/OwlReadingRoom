using OwlReadingRoom.Models;
using OwlReadingRoom.Services.Database;
using OwlReadingRoom.Utils;
using OwlReadingRoom.ViewModels;
using System.Diagnostics;

namespace OwlReadingRoom.Services.Resources;

public class ResourceService : IPhysicalResourceService
{
    private readonly IRoomService _roomService;
    private readonly IDeskService _deskService;
    private readonly IBookingService _bookingService;
    private readonly IDatabaseConnectionService _databaseConnectionService;

    public ResourceService(IRoomService roomService, IDeskService deskService, IBookingService bookingService, IDatabaseConnectionService databaseConnectionService)
    {
        _roomService = roomService;
        _deskService = deskService;
        _bookingService = bookingService;
        _databaseConnectionService = databaseConnectionService;
    }

    public List<RoomListViewModel> fetchRooms()
    {

        var roomDeskCounts = from room in _roomService.TableQuery
                             join desk in _deskService.TableQuery on room.Id equals desk.RoomId into deskGroup
                             select new
                             {
                                 RoomId = room.Id,
                                 RoomName = room.Name,
                                 RoomType = room.RoomType,
                                 Desks = deskGroup.Count(d => d != null)
                             };

        Dictionary<int, int> unavailableDesks = _bookingService.FetchUnavailableDesksCount();
        List<RoomListViewModel> result = new List<RoomListViewModel>();

        foreach(var room in roomDeskCounts)
        {
            result.Add(new RoomListViewModel
            {
                Id = room.RoomId,
                Name = room.RoomName,
                RoomType = room.RoomType == RoomType.AC ? "AC Room" : "Non-AC room",
                TotalDesks = room.Desks,
                AvailableDesks = room.Desks - unavailableDesks.GetValueOrDefault(room.RoomId, 0)
            });
        }
        
        return result;
    }

    public void UpdateRoom(RoomListViewModel roomViewModel, string roomName, int numberOfDesks, string deskInitial)
    {
        using (_databaseConnectionService)
        {
            try
            {
                _databaseConnectionService.BeginTransaction();
                if (numberOfDesks > 0)
                {
                    _deskService.AddDesks(roomViewModel.Id, numberOfDesks, deskInitial);
                }
              
                Room room = _roomService.GetRoomById(roomViewModel.Id);

                if (!room.Name.Equals(roomName))
                {
                    room.Name = roomName;
                    _roomService.Save(room);
                }
                _databaseConnectionService.CommitTransaction();

            }
            catch (Exception ex)
            {
                _databaseConnectionService.RollBack();
                ExceptionHandler.HandleException("Updating room information", ex);
            }

        }
    }

}
