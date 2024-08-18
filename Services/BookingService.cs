using OwlReadingRoom.DTOs;
using OwlReadingRoom.Models;
using OwlReadingRoom.Services.Database;
using OwlReadingRoom.Services.Repository;
using System.Diagnostics;

namespace OwlReadingRoom.Services;

public class BookingService : IBookingService
{
    private readonly ICustomerService _customerService;
    private readonly IDatabaseConnectionService _databaseConnectionService;
    private readonly IRepository<BookingInfo> _bookingRepository;
    public BookingService(ICustomerService customerService, IDatabaseConnectionService databaseConnectionService, IRepository<BookingInfo> bookingRepository)
    {
        _customerService = customerService;
        _databaseConnectionService = databaseConnectionService;
        _bookingRepository = bookingRepository;
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
}
