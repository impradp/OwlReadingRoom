using OwlReadingRoom.DTOs;
using OwlReadingRoom.Models;
using OwlReadingRoom.Models.Enums;
using OwlReadingRoom.Proxy;
using OwlReadingRoom.Services.Constants;
using OwlReadingRoom.Services.Repository;
using OwlReadingRoom.Services.Resources;
using OwlReadingRoom.Services.Transactions;
using OwlReadingRoom.ViewModels;
using SQLite;
using System.Diagnostics;

namespace OwlReadingRoom.Services;

public class BookingDetailsService : IBookingService
{
    private readonly ICustomerService _customerService;
    private readonly IRepository<BookingInfo> _bookingRepository;
    private readonly IDeskService _deskService;
    private readonly IPackageService _packageService;
    private readonly IRoomService _roomService;
    private readonly ITransactionService _transactionService;
    public BookingDetailsService(ICustomerService customerService, IRepository<BookingInfo> bookingRepository, IDeskService deskService, IPackageService packageService, IRoomService roomService, ITransactionService transactionService)
    {
        _customerService = customerService;
        _bookingRepository = bookingRepository;
        _deskService = deskService;
        _packageService = packageService;
        _roomService = roomService;
        _transactionService = transactionService;
    }

    public Dictionary<int, int> FetchUnavailableDesksCount()
    {
        var currentTime = DateTime.Now;
        return (from desk in _deskService.TableQuery
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
        TableQuery<Customer> customerQuery = _customerService.CustomerTableQuery;
        TableQuery<PersonalDetail> personalDetailsQuery = _customerService.PersonalDetailTableQuery;

        var query = from desk in _deskService.TableQuery
                    join booking in _bookingRepository.Table
                    on desk.Id equals booking.DeskId
                    join customer in customerQuery
                    on booking.CustomerId equals customer.Id
                    join personalDetail in personalDetailsQuery
                    on customer.Id equals personalDetail.CustomerId
                    where desk.RoomId == roomId && booking.ReservationEndDate >= currentTime
                    orderby booking.ReservationStartDate
                    group new
                    {
                        CustomerID = booking.CustomerId,
                        CustomerFullName = personalDetail.FullName,
                        StartDate = booking.ReservationStartDate?.ToShortDateString(),
                        EndDate = booking.ReservationEndDate?.ToShortDateString(),
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

    public List<CustomerPackageViewModel> GetActiveCustomerList()
    {
        var currentTime = DateTime.Now;
        TableQuery<Customer> customerQuery = _customerService.CustomerTableQuery;
        TableQuery<PackageType> packageQuery = _packageService.TableQuery;
        TableQuery<PersonalDetail> personalDetailsQuery = _customerService.PersonalDetailTableQuery;
        TableQuery<Room> roomQuery = _roomService.TableQuery;
        TableQuery<Transaction> transactionQuery = _transactionService.TableQuery;


        // Step 1: Group bookings by customer and get the latest booking (based on ReservationEndDate)
        var latestBookingsQuery = from booking in _bookingRepository.Table
                                  group booking by booking.CustomerId into bookingGroup
                                  select new
                                  {
                                      CustomerId = bookingGroup.Key,
                                      LatestBooking = bookingGroup
                                          .OrderByDescending(b => b.Id)
                                          .FirstOrDefault()
                                  };

        // Step 2: Join the necessary tables, filter for active customers, and populate CustomerPackageViewModel
        var query = from customer in customerQuery
                    join personalDetail in personalDetailsQuery on customer.Id equals personalDetail.CustomerId
                    join latestBooking in latestBookingsQuery on customer.Id equals latestBooking.CustomerId
                    join desk in _deskService.TableQuery on latestBooking.LatestBooking.DeskId equals desk.Id
                    join room in roomQuery on desk.RoomId equals room.Id
                    join package in packageQuery on latestBooking.LatestBooking.PackageId equals package.Id
                    join transaction in transactionQuery on latestBooking.LatestBooking.Id equals transaction.BookingInformationId
                    where latestBooking.LatestBooking.ReservationEndDate >= currentTime
                    orderby personalDetail.FullName
                    select new CustomerPackageViewModel
                    {
                        CustomerId = customer.Id,
                        BookingId = latestBooking.LatestBooking.Id,
                        FullName = personalDetail.FullName,
                        ContactNumber = customer.MobileNumber,
                        Faculty = personalDetail.Faculty,
                        StartDate = latestBooking.LatestBooking.ReservationStartDate,
                        EndDate = latestBooking.LatestBooking.ReservationEndDate,
                        Package = package.Name,
                        AllocatedSpace = $"{room.Name} ({desk.Name})",
                        Dues = transaction.DueAmount.ToString(),
                        PaymentStatus = transaction?.PaymentStatus,
                        Status = Status.ACTIVE
                    };
        return query.ToList();
    }

    public List<InactiveCustomerDetail> GetToBeInactiveCustomerList()
    {
        DateTime currentDate = DateTime.Now;
        DateTime expiryThresholdDate = currentDate.AddDays(AppConstants.Notification.DaysUntilExpiration).Date;
        TableQuery<Customer> customerQuery = _customerService.CustomerTableQuery;
        TableQuery<PackageType> packageQuery = _packageService.TableQuery;
        TableQuery<PersonalDetail> personalDetailsQuery = _customerService.PersonalDetailTableQuery;
        // Step 1: Group bookings by customer and get the latest booking (if exists)
        var latestBookingsQuery = from booking in _bookingRepository.Table
                                  group booking by booking.CustomerId into bookingGroup
                                  select new
                                  {
                                      CustomerId = bookingGroup.Key,
                                      LatestBooking = bookingGroup
                                          .OrderByDescending(b => b.Id)
                                          .FirstOrDefault()
                                  };
        var query = from customer in customerQuery
                    join personalDetail in personalDetailsQuery on customer.Id equals personalDetail.CustomerId
                    join latestBooking in latestBookingsQuery on customer.Id equals latestBooking.CustomerId
                    join package in packageQuery on latestBooking?.LatestBooking?.PackageId equals package?.Id
                    where latestBooking.LatestBooking.ReservationEndDate?.Date > currentDate.Date && // Package is not expired yet
                     latestBooking.LatestBooking.ReservationEndDate?.Date <= expiryThresholdDate // Package expires within 5 days
                    select new InactiveCustomerDetail
                    {
                        FullName = personalDetail.FullName,
                        Mobile = customer.MobileNumber,
                        PackageName = package.Name,
                        ExpiryDate = latestBooking.LatestBooking.ReservationEndDate,
                    };
        return query.ToList();
    }

    public List<CustomerPackageViewModel> GetInactiveCustomerList()
    {
        var currentTime = DateTime.Now;
        TableQuery<Customer> customerQuery = _customerService.CustomerTableQuery;
        TableQuery<PackageType> packageQuery = _packageService.TableQuery;
        TableQuery<PersonalDetail> personalDetailsQuery = _customerService.PersonalDetailTableQuery;
        TableQuery<Room> roomQuery = _roomService.TableQuery;
        TableQuery<Transaction> transactionQuery = _transactionService.TableQuery;

        // Step 1: Group bookings by customer and get the latest booking (if exists)
        var latestBookingsQuery = from booking in _bookingRepository.Table
                                  group booking by booking.CustomerId into bookingGroup
                                  select new
                                  {
                                      CustomerId = bookingGroup.Key,
                                      LatestBooking = bookingGroup
                                          .OrderByDescending(b => b.Id)
                                          .FirstOrDefault()
                                  };

        // Step 2: Perform a left join and filter customers with no booking or with an expired booking
        var query = from customer in customerQuery
                    join personalDetail in personalDetailsQuery on customer.Id equals personalDetail.CustomerId
                    join latestBooking in latestBookingsQuery
                    on customer.Id equals latestBooking.CustomerId
                    join desk in _deskService.TableQuery on latestBooking?.LatestBooking?.DeskId equals desk.Id into deskGroup
                    from desk in deskGroup.DefaultIfEmpty() // Handle null desk if no bookings
                    join room in roomQuery on desk?.RoomId equals room?.Id into roomGroup
                    from room in roomGroup.DefaultIfEmpty() // Handle null room if no bookings
                    join package in packageQuery on latestBooking?.LatestBooking?.PackageId equals package?.Id into packageGroup
                    from package in packageGroup.DefaultIfEmpty() // Handle null package if no bookings
                    join transaction in transactionQuery on latestBooking?.LatestBooking?.Id equals transaction?.BookingInformationId into transactionGroup
                    from transaction in transactionGroup.DefaultIfEmpty() // Handle null transaction if no bookings
                    where latestBooking.LatestBooking.ReservationEndDate == null || latestBooking.LatestBooking?.ReservationEndDate < currentTime // Customers with no bookings or expired bookings
                    orderby personalDetail.FullName
                    select new CustomerPackageViewModel
                    {
                        CustomerId = customer.Id,
                        BookingId = latestBooking.LatestBooking.Id,
                        FullName = personalDetail.FullName,
                        ContactNumber = customer.MobileNumber,
                        Faculty = personalDetail.Faculty,
                        StartDate = latestBooking.LatestBooking.ReservationStartDate,
                        EndDate = latestBooking.LatestBooking.ReservationEndDate,
                        Package = package != null ? package.Name : "N/A",
                        AllocatedSpace = latestBooking.LatestBooking.PackageId != null ? $"{room?.Name ?? "N/A"} - {desk?.Name ?? "N/A"}" : "N/A",
                        PaymentStatus = transaction?.PaymentStatus,
                        Dues = transaction != null ? transaction.DueAmount.ToString() : "0",
                        Status = Status.INACTIVE
                    };

        return query.ToList();
    }

    public BookingInfoViewModel GetBookingDetails(int? bookingId)
    {
        if (!bookingId.HasValue)
        {
            return null;
        }

        var query = from booking in _bookingRepository.Table
                    join package in _packageService.TableQuery on booking.PackageId equals package.Id into packageGroup
                    from pacakgeInfo in packageGroup.DefaultIfEmpty()
                    join desk in _deskService.TableQuery on booking.DeskId equals desk.Id into deskGroup
                    from deskInfo in deskGroup.DefaultIfEmpty()
                    join room in _roomService.TableQuery on deskInfo?.RoomId equals room.Id into roomGroup
                    from room in roomGroup.DefaultIfEmpty()
                    where booking.Id == bookingId
                    select new BookingInfoViewModel
                    {
                        Id = booking.Id,
                        RoomType = pacakgeInfo?.RoomType,
                        PackageName = pacakgeInfo?.Name,
                        StartDate = booking.ReservationStartDate,
                        EndDate = booking.ReservationEndDate,
                        DeskName = deskInfo?.Name,
                        PackagePrice = pacakgeInfo?.Price ?? 0,
                        RoomName = room?.Name
                    };
        return query.FirstOrDefault();
    }

    public bool ContainsActiveBookingWithPackageId(int? packageId)
    {
        if (!packageId.HasValue)
        {
            return false;
        }

        return GetActiveBookingsForPackage(packageId.Value).Any();
    }

    /// <summary>
    /// Retrieves a list of active booking details for a specific package.
    /// </summary>
    /// <param name="packageId">The ID of the package to retrieve bookings for.</param>
    /// <returns>A list of active booking details for the specified package.</returns>
    private List<BookingInfo> GetActiveBookingsForPackage(int? packageId)
    {
        var currentTime = DateTime.Now;
        return _bookingRepository.Table.Where(booking => booking.PackageId == packageId && booking.ReservationEndDate >= currentTime).ToList();
    }

    public BookingInfo GetBookingDetailsById(int? bookingId)
    {
        return _bookingRepository.GetItem(bookingId);
    }

    [Transactional]
    public void PerformInitialBooking(MinimumCustomerDetail minimumCustomerDetail)
    {
        Customer customer = _customerService.RegisterWithMinimumDetails(minimumCustomerDetail);
        _bookingRepository.SaveItem(new BookingInfo
        {
            CustomerId = customer.Id,
        });
    }

    private bool isNewPackage(BookingInfo bookingInfo)
    {
        return bookingInfo.PackageId == null;
    }

    public void SaveItem(BookingInfo bookingInfo)
    {
        _bookingRepository.SaveItem(bookingInfo);
    }

    public class ReservationInfo
    {
        public int CustomerID { get; set; }
        public string CustomerFullName { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }

        public DeskStatus Status { get; set; }
    }

    [Transactional]
    public int PerformCustomerRenew(int customerId)
    {
        return _bookingRepository.SaveItem(new BookingInfo
        {
            CustomerId = customerId
        });
    }
}
