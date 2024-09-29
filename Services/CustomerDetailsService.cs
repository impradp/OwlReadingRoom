
using OwlReadingRoom.Models;
using OwlReadingRoom.Services.Transactions;
using OwlReadingRoom.ViewModels;

namespace OwlReadingRoom.Services;

public class CustomerDetailsService : ICustomerDetailsService
{
    private readonly ICustomerService _customerService;
    private readonly IBookingService _bookingService;
    private readonly ITransactionService _transactionService;
    public CustomerDetailsService(ICustomerService customerService, IBookingService bookingService, ITransactionService transactionService)
    {
        _customerService = customerService;
        _bookingService = bookingService;
        _transactionService = transactionService;
    }

    public CustomerDetailViewModel fetchCustomerDetails(CustomerPackageViewModel minimumInformation)
    {
        var personalDetail = _customerService.GetPersonalDetails(minimumInformation.CustomerId);
        var addressDetail = _customerService.GetAddressDetails(minimumInformation.CustomerId);
        var documentInformation = _customerService.GetCustomerDocumentDetails(minimumInformation.CustomerId);
        var bookingInformation = _bookingService.GetBookingDetails(minimumInformation.BookingId);
        var transactionInformation = _transactionService.GetTransactionDetails(minimumInformation.BookingId);
        return new CustomerDetailViewModel
        {
            CustomerId = minimumInformation.CustomerId,
            FullName = personalDetail.FullName,
            Allergies = personalDetail.Allergies,
            CurrentAddress = addressDetail.TemporaryAddress,
            PermanantAddress = addressDetail.PermanentAddress,
            DateOfBirth = personalDetail.DOB,
            Disease = personalDetail.Disease,
            Gender = personalDetail.Gender,
            Faculty = personalDetail.Faculty,
            MobileNumber = minimumInformation.ContactNumber,
            BookingDetails = bookingInformation,
            TransactionDetails = transactionInformation,
            Documents = documentInformation,
            Status = minimumInformation.Status.ToString()
        };
    }

    public void RenewCustomerBooking(CustomerDetailViewModel customerDetail)
    {
        int bookingId = _bookingService.PerformCustomerRenew(customerDetail.CustomerId);
        customerDetail.BookingDetails = new BookingInfoViewModel()
        {
            Id = bookingId,
        };
    }
}
