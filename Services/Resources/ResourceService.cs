using OwlReadingRoom.Models;
using OwlReadingRoom.Models.Enums;
using OwlReadingRoom.Proxy;
using OwlReadingRoom.Services.Database;
using OwlReadingRoom.Utils;
using OwlReadingRoom.ViewModels;
using static OwlReadingRoom.Services.BookingService;

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

    [Transactional]
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

    public List<DeskInfoViewModel> GetDeskInfoPerRoom(int roomId)
    {
        List<Desk> desks = _deskService.GetDesksOfRoom(roomId);
        Dictionary<int, List<ReservationInfo>> reservation = _bookingService.GetDeskBookingInformation(roomId);
        List<DeskInfoViewModel> deskInfoViewModels = new List<DeskInfoViewModel>();

        foreach (var desk in desks)
        {
            if (reservation.TryGetValue(desk.Id, out var reservations) && reservations.Any())
            {
                // Use the first reservation for this desk
                var firstReservation = reservations.First();

                deskInfoViewModels.Add(new DeskInfoViewModel
                {
                    Name = $"{desk.Name} ",
                    Status = firstReservation.Status,
                    Color = Utility.GetBackGroundColorByDeskStatus(firstReservation.Status),
                    TextColor = Utility.GetTextColorByDeskStatus(firstReservation.Status),
                    Message = $"Reserved by {firstReservation.CustomerFullName} from {firstReservation.StartDate} to {firstReservation.EndDate}"
                });
            }
            else
            {
                // No reservation, use default values
                deskInfoViewModels.Add(new DeskInfoViewModel
                {
                    Name = $"{desk.Name} ",
                    Status = DeskStatus.Available, // Assuming 'Available' is a default status
                    Color = Utility.GetBackGroundColorByDeskStatus(DeskStatus.Available),
                    TextColor = Utility.GetTextColorByDeskStatus(DeskStatus.Available),
                    Message = "Available"
                });
            }
        }
        return deskInfoViewModels;
    }

}
