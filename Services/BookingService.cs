using OwlReadingRoom.DTOs;
using OwlReadingRoom.Models;
using OwlReadingRoom.Services.Database;
using OwlReadingRoom.Services.Repository;
using OwlReadingRoom.Services.Resources;
using System.Diagnostics;

namespace OwlReadingRoom.Services;

public class BookingService : IBookingService
{
    private readonly ICustomerService _customerService;
    private readonly IDatabaseConnectionService _databaseConnectionService;
    private readonly IRepository<BookingInfo> _bookingRepository;
    private readonly IDeskService _deskService;
    public BookingService(ICustomerService customerService, IDatabaseConnectionService databaseConnectionService, IRepository<BookingInfo> bookingRepository, IDeskService deskService)
    {
        _customerService = customerService;
        _databaseConnectionService = databaseConnectionService;
        _bookingRepository = bookingRepository;
        _deskService = deskService;
    }
    public void RegisterWithMinimumDetails(MinimumCustomerDetail minimumCutomerDetail)
    {
        using (_databaseConnectionService)
        {
            try
            {
                //TODO: validate the booking 
                _databaseConnectionService.BeginTransaction();

                int cusomterId = _customerService.CreateNewCustomerWithMinimumDetails(minimumCutomerDetail);
                /*_bookingService.BookCustomer();*/
                BookingInfo bookingInfo = new BookingInfo()
                {
                    CustomerId = cusomterId,
                    PackageId = minimumCutomerDetail.PackageType
                };
                _bookingRepository.SaveItem(bookingInfo);
                _databaseConnectionService.CommitTransaction();

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                _databaseConnectionService.RollBack();
            }     
        }
        

    }

    public Dictionary<int, int> FetchUnavailableDesksCount()
    {
        var currentTime = DateTime.Now;
        return  (from desk in _deskService.TableQuery
                    join booking in _bookingRepository.Table on desk.Id equals booking.DeskId
                    where booking.ReservationStartDate <= currentTime && booking.ReservationEndDate >= currentTime
                    group desk by desk.RoomId into deskGroup
                    select new
                    {
                        RoomId = deskGroup.Key,
                        UnavailableDesksCount = deskGroup.Count()
                    }).ToDictionary(k => k.RoomId, v => v.UnavailableDesksCount);

    }
}
