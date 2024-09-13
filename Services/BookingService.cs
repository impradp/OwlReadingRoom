using OwlReadingRoom.DTOs;
using OwlReadingRoom.Models;
using OwlReadingRoom.Models.Enums;
using OwlReadingRoom.Proxy;
using OwlReadingRoom.Services.Repository;
using OwlReadingRoom.Services.Resources;
using OwlReadingRoom.Services.Transactions;
using OwlReadingRoom.ViewModels;
using SQLite;
using System.Diagnostics;

namespace OwlReadingRoom.Services;

public class BookingService : IBookingService
{
    private readonly ICustomerService _customerService;
    private readonly IRepository<BookingInfo> _bookingRepository;
    private readonly IDeskService _deskService;
    private readonly IPackageService _packageService;
    private readonly IRoomService _roomService;
    private readonly ITransactionService _transactionService;
    public BookingService(ICustomerService customerService, IRepository<BookingInfo> bookingRepository, IDeskService deskService, IPackageService packageService, IRoomService roomService, ITransactionService transactionService)
    {
        _customerService = customerService;
        _bookingRepository = bookingRepository;
        _deskService = deskService;
        _packageService = packageService;
        _roomService = roomService;
        _transactionService = transactionService;
    }

    [Transactional]
    public void RegisterWithMinimumDetails(MinimumCustomerDetail minimumCutomerDetail)
    {
        Customer customer = _customerService.GetCustomerByMobileNumber(minimumCutomerDetail.ContactNumber);
        if(customer is not null)
        {
            Debug.WriteLine($"A customer with the contact number :{minimumCutomerDetail.ContactNumber} already exists.");
            throw new InvalidDataException("Duplicate user entry.");
        }
        _customerService.CreateNewCustomerWithMinimumDetails(minimumCutomerDetail);
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

    public List<CustomerPackageViewModel> GetActiveCustomerList()
    {
        var currentTime = DateTime.Now;
        TableQuery<Customer> customerQuery = _customerService.CustomerTableQuery;
        TableQuery<PackageType> packageQuery = _packageService.TableQuery;
        TableQuery<PersonalDetail> personalDetailsQuery = _customerService.PersonalDetailTableQuery;
        TableQuery<Room> roomQuery = _roomService.TableQuery;
        TableQuery<Transaction> transactionQuery = _transactionService.TableQuery;
        TableQuery<DocumentInformation> documentQuery = _customerService.DocumentTableQuery;
        TableQuery<DocumentImage> documentImageQuery = _customerService.DocumentImageTableQuery;


        // Step 1: Group bookings by customer and get the latest booking (based on ReservationEndDate)
        var latestBookingsQuery = from booking in _bookingRepository.Table
                                  group booking by booking.CustomerId into bookingGroup
                                  select new
                                  {
                                      CustomerId = bookingGroup.Key,
                                      LatestBooking = bookingGroup
                                          .OrderByDescending(b => b.ReservationEndDate)
                                          .FirstOrDefault()
                                  };

        // Step 2: Join the necessary tables, filter for active customers, and populate CustomerPackageViewModel
        var query = from customer in customerQuery
                    join personalDetail in personalDetailsQuery on customer.Id equals personalDetail.CustomerId
                    join latestBooking in latestBookingsQuery on customer.Id equals latestBooking.CustomerId
                    join desk in _deskService.TableQuery on latestBooking.LatestBooking.DeskId equals desk.Id
                    join room in roomQuery on desk.RoomId equals room.Id
                    join package in packageQuery on latestBooking.LatestBooking.PackageId equals package.Id
                    join transaction in transactionQuery on latestBooking.LatestBooking.TransactionId equals transaction.Id into txnGroup
                    from txn in txnGroup.DefaultIfEmpty()
                    join docInfo in documentQuery on customer.Id equals docInfo.CustomerId into docGroup
                    from document in docGroup.DefaultIfEmpty()
                    where latestBooking.LatestBooking.ReservationEndDate >= currentTime
                    orderby customer.Id
                    select new CustomerPackageViewModel
                    {
                        FullName = personalDetail.FullName,
                        ContactNumber = customer.MobileNumber,
                        Faculty = personalDetail.Faculty,
                        StartDate = latestBooking.LatestBooking.ReservationStartDate.ToShortDateString(),
                        EndDate = latestBooking.LatestBooking.ReservationEndDate.ToShortDateString(),
                        Package = package.Name,
                        AllocatedSpace = $"{room.Name} - {desk.Name}",
                        PaymentStatus = txn?.PaymentStatus.ToString() ?? PaymentStatusEnum.UNPAID.ToString(),
                        Dues = txn?.Dues.ToString() ?? package?.Price.ToString() ?? "N/A",
                        Documents = document != null ? new DocumentViewModel
                                     {
                                         Id = document.Id,
                                         DocumentNumber = document.DocumentNumber,
                                         IssueDate = document.IssueDate,
                                         DocumentType = document.DocumentType,
                                         Locations = (from image in documentImageQuery
                                                      where image.DocumentInformationId == document.Id
                                                      select new DocumentImageViewModel
                                                      {
                                                          Id = image.Id,
                                                          ImagePath = image.ImagePath
                                                      }).ToList()
                        } : new DocumentViewModel() // Retrieves the list of documents for each customer
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
        TableQuery<DocumentInformation> documentQuery = _customerService.DocumentTableQuery;
        TableQuery<DocumentImage> documentImageQuery = _customerService.DocumentImageTableQuery;

        //TODO: order latest reservation by id
        // Step 1: Group bookings by customer and get the latest booking (if exists)
        var latestBookingsQuery = from booking in _bookingRepository.Table
                                  group booking by booking.CustomerId into bookingGroup
                                  select new
                                  {
                                      CustomerId = bookingGroup.Key,
                                      LatestBooking = bookingGroup
                                          .OrderByDescending(b => b.ReservationEndDate)
                                          .FirstOrDefault()
                                  };

        // Step 2: Perform a left join and filter customers with no booking or with an expired booking
        var query = from customer in customerQuery
                    join personalDetail in personalDetailsQuery on customer.Id equals personalDetail.CustomerId
                    join latestBooking in latestBookingsQuery
                    on customer.Id equals latestBooking.CustomerId into customerBookings
                    from latestBooking in customerBookings.DefaultIfEmpty() // Left join: include customers with no bookings
                    join docInfo in documentQuery on customer.Id equals docInfo.CustomerId into docGroup
                    from document in docGroup.DefaultIfEmpty()
                    join desk in _deskService.TableQuery on latestBooking?.LatestBooking?.DeskId equals desk.Id into deskGroup
                    from desk in deskGroup.DefaultIfEmpty() // Handle null desk if no bookings
                    join room in roomQuery on desk?.RoomId equals room?.Id into roomGroup
                    from room in roomGroup.DefaultIfEmpty() // Handle null room if no bookings
                    join package in packageQuery on latestBooking?.LatestBooking?.PackageId equals package?.Id into packageGroup
                    from package in packageGroup.DefaultIfEmpty() // Handle null package if no bookings
                    join transaction in transactionQuery on latestBooking?.LatestBooking?.TransactionId equals transaction?.Id into transactionGroup
                    from transaction in transactionGroup.DefaultIfEmpty() // Handle null transaction if no bookings
                    where latestBooking == null || latestBooking?.LatestBooking?.ReservationEndDate < currentTime // Customers with no bookings or expired bookings
                    orderby customer.Id
                    select new CustomerPackageViewModel
                    {
                        FullName = personalDetail.FullName,
                        ContactNumber = customer.MobileNumber,
                        Faculty = personalDetail.Faculty,
                        StartDate = latestBooking != null ? latestBooking.LatestBooking.ReservationStartDate.ToShortDateString() : "N/A",
                        EndDate = latestBooking != null ? latestBooking.LatestBooking.ReservationEndDate.ToShortDateString() : "N/A",
                        Package = package != null ? package.Name : "N/A",
                        AllocatedSpace = latestBooking != null ? $"{room?.Name ?? "N/A"} - {desk?.Name ?? "N/A"}" : "N/A",
                        PaymentStatus = transaction != null ? transaction.PaymentStatus.ToString() : PaymentStatusEnum.UNPAID.ToString(),
                        Dues = transaction != null ? transaction.Dues.ToString() : package?.Price.ToString() ?? "N/A",
                        Documents = document != null ? new DocumentViewModel
                        {
                            Id = document.Id,
                            DocumentNumber = document.DocumentNumber,
                            IssueDate = document.IssueDate,
                            DocumentType = document.DocumentType,
                            Locations = (from image in documentImageQuery
                                         where image.DocumentInformationId == document.Id
                                         select new DocumentImageViewModel
                                         {
                                             Id = image.Id,
                                             ImagePath = image.ImagePath
                                         }).ToList()
                        } : new DocumentViewModel()  // Retrieves the list of documents for each customer
                    };

        return query.ToList();
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
