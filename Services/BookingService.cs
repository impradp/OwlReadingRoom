using OwlReadingRoom.DTOs;
using OwlReadingRoom.Models;
using OwlReadingRoom.Models.Enums;
using OwlReadingRoom.Services.Database;
using OwlReadingRoom.Services.Repository;
using OwlReadingRoom.Services.Resources;
using SQLite;
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

    public Dictionary<int, List<ReservationInfo>> GetDeskBookingInformation(int roomId)
    {
        var currentTime = DateTime.Now;
        TableQuery<Customer> customerQuery = _databaseConnectionService.Connection.Table<Customer>();
        TableQuery<PersonalDetail> personalDetailsQuery = _databaseConnectionService.Connection.Table<PersonalDetail>();

        var query = from desk in _deskService.TableQuery
                    join booking in _bookingRepository.Table
                    on desk.Id equals booking.DeskId
                    join customer in customerQuery
                    on booking.CustomerId equals customer.Id
                    join personalDetail in personalDetailsQuery
                    on customer.PersonalDetailId equals personalDetail.Id
                    where desk.RoomId == roomId && booking.ReservationEndDate >= currentTime
                    orderby booking.ReservationStartDate
                    group new
                    {
                        CustomerID = booking.CustomerId,
                        CustomerFullName = personalDetail.FullName,
                        StartDate = booking.ReservationStartDate.ToShortDateString(),
                        EndDate = booking.ReservationEndDate.ToShortDateString(),
                        Status = booking.ReservationStartDate <= currentTime ? DeskStatus.NotAvailable : DeskStatus.Reserved
                    } by desk.Id into deskGroup
                    select new 
                    {
                        DeskID = deskGroup.Key,
                        Reservations = (from d in deskGroup
                        select new ReservationInfo
                        {
                            CustomerID = d.CustomerID,
                            CustomerFullName = d.CustomerFullName,
                            StartDate = d.StartDate,
                            EndDate = d.EndDate, 
                            Status = d.Status
                        }).ToList()
                    };
        return query.ToDictionary(key => key.DeskID, value => value.Reservations);
    }
    public class ReservationInfo
    {
        public int CustomerID { get; set; }
        public string CustomerFullName { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }

        public DeskStatus Status {  get; set; }
    }
}
