using OwlReadingRoom.Models;
using OwlReadingRoom.Models.Enums;
using OwlReadingRoom.Proxy;
using OwlReadingRoom.Services.Constants;
using OwlReadingRoom.Services.Database;
using OwlReadingRoom.Utils;
using OwlReadingRoom.ViewModels;
using static OwlReadingRoom.Services.BookingDetailsService;

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

    [Transactional(readOnly: true)]
    public List<RoomListViewModel> fetchRooms()
    {

        var roomDeskCounts = from room in _roomService.TableQuery
                             join desk in _deskService.TableQuery on room.Id equals desk.RoomId into deskGroup
                             orderby room.RoomType ascending
                             select new
                             {
                                 RoomId = room.Id,
                                 RoomName = room.Name,
                                 RoomType = room.RoomType,
                                 Desks = deskGroup.Count(d => d != null)
                             };

        Dictionary<int, int> unavailableDesks = _bookingService.FetchUnavailableDesksCount();
        List<RoomListViewModel> result = new List<RoomListViewModel>();

        foreach (var room in roomDeskCounts)
        {
            result.Add(new RoomListViewModel
            {
                Id = room.RoomId,
                Name = room.RoomName,
                RoomType = room.RoomType == RoomType.AC ? AppConstants.RoomConstants.AcRoom : AppConstants.RoomConstants.NonAcRoom,
                TotalDesks = room.Desks,
                AvailableDesks = room.Desks - unavailableDesks.GetValueOrDefault(room.RoomId, 0)
            });
        }

        return result;
    }

    [Transactional(readOnly: true)]
    public List<RoomListViewModel> FetchRoomsByType(RoomType roomType)
    {

        var roomDeskCounts = from room in _roomService.TableQuery
                             join desk in _deskService.TableQuery on room.Id equals desk.RoomId into deskGroup
                             where room.RoomType == roomType
                             select new
                             {
                                 RoomId = room.Id,
                                 RoomName = room.Name,
                                 RoomType = room.RoomType,
                                 Desks = deskGroup.Count(d => d != null)
                             };

        Dictionary<int, int> unavailableDesks = _bookingService.FetchUnavailableDesksCount();
        List<RoomListViewModel> result = new List<RoomListViewModel>();

        foreach (var room in roomDeskCounts)
        {
            result.Add(new RoomListViewModel
            {
                Id = room.RoomId,
                Name = room.RoomName,
                RoomType = room.RoomType == RoomType.AC ? AppConstants.RoomConstants.AcRoom : AppConstants.RoomConstants.NonAcRoom,
                TotalDesks = room.Desks,
                AvailableDesks = room.Desks - unavailableDesks.GetValueOrDefault(room.RoomId, 0)
            });
        }

        return result;
    }


    [Transactional(readOnly: true)]
    public RoomListViewModel FetchRoomById(int id)
    {

        var roomResult = (from room in _roomService.TableQuery
                          join desk in _deskService.TableQuery on room.Id equals desk.RoomId into deskGroup
                          where room.Id == id
                          select new
                          {
                              RoomId = room.Id,
                              RoomName = room.Name,
                              RoomType = room.RoomType,
                              Desks = deskGroup.Count(d => d != null)
                          }).FirstOrDefault();

        Dictionary<int, int> unavailableDesks = _bookingService.FetchUnavailableDesksCount();

        RoomListViewModel result = new RoomListViewModel
        {
            Id = roomResult.RoomId,
            Name = roomResult.RoomName,
            RoomType = roomResult.RoomType == RoomType.AC ? AppConstants.RoomConstants.AcRoom : AppConstants.RoomConstants.NonAcRoom,
            TotalDesks = roomResult.Desks,
            AvailableDesks = roomResult.Desks - unavailableDesks.GetValueOrDefault(roomResult.RoomId, 0)
        };

        return result;
    }

    [Transactional]
    public void UpdateRoom(RoomListViewModel roomViewModel, string roomName)
    {
        Room room = _roomService.GetRoomById(roomViewModel.Id);

        if (!room.Name.Equals(roomName))
        {
            room.Name = roomName;
            _roomService.Save(room);
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
                    Id = desk.Id,
                    Name = $"{desk.Name}",
                    Status = firstReservation.Status,
                    Color = Utility.GetBackGroundColorByDeskStatus(firstReservation.Status),
                    Message = $"Reserved by {firstReservation.CustomerFullName} from {firstReservation.StartDate} to {firstReservation.EndDate}"
                });
            }
            else
            {
                // No reservation, use default values
                deskInfoViewModels.Add(new DeskInfoViewModel
                {
                    Id = desk.Id,
                    Name = $"{desk.Name}",
                    Status = DeskStatus.Available, // Assuming 'Available' is a default status
                    Color = Utility.GetBackGroundColorByDeskStatus(DeskStatus.Available),
                    Message = "Available"
                });
            }
        }
        return deskInfoViewModels;
    }

    [Transactional]
    public void AddRooms(RoomType roomType, int? numberOfRooms, string roomInitials = "RM")
    {
        var roomDeskNames = roomType == RoomType.NON_AC ? RoomPlan.Non_AC_Room_Desks : RoomPlan.AC_Room_Desks;

        List<Room> rooms = _roomService.AddRooms(roomType, numberOfRooms, roomInitials);

        var roomIds = rooms.Select(r => r.Id).ToList();

        var desks = new List<Desk>();
        foreach (var roomId in roomIds)
        {
            desks.AddRange(roomDeskNames.Select(deskName => new Desk { RoomId = roomId, Name = deskName.Trim() }));
        }

        _deskService.AddDesks(desks);
    }
}
