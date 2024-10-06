
using OwlReadingRoom.DTOs;
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

    public MinimumCustomerDetail GetCustomerMinimumDetail(int customerId)
    {  
        PersonalDetail personalDetail = _customerService.GetPersonalDetails(customerId);
        return new MinimumCustomerDetail
        {
            FullName = FormatFullNameWithHonorific(personalDetail.Gender, personalDetail.FullName)
        };
    }

    /// <summary>
    /// Formats the full name of the customer by adding an appropriate honorific based on gender.
    /// </summary>
    /// <param name="gender">The gender of the customer, which may be null or empty.</param>
    /// <param name="fullName">The full name of the customer.</param>
    /// <returns>A string representing the full name prefixed with the appropriate honorific.</returns>
    private string FormatFullNameWithHonorific(string gender, string fullName)
    {
        string honorific = GetHonorific(gender);
        return $"{honorific}{fullName}";
    }

    /// <summary>
    /// Retrieves the appropriate honorific based on the customer's gender.
    /// </summary>
    /// <param name="gender">The gender of the customer. Can be null or empty.</param>
    /// <returns>A string representing the honorific (e.g., "Mr.", "Ms.", "Mx.") or an empty string if no valid gender is provided.</returns>
    private string GetHonorific(string gender)
    {
        // Handle null or empty gender directly
        if (string.IsNullOrEmpty(gender))
        {
            return string.Empty; // Return empty string if gender is null or empty
        }

        var honorifics = new Dictionary<string, string>
    {
        { "Male", "Mr. " },
        { "Female", "Ms. " },
        { "Other", "Mx. " }
    };

        return honorifics.TryGetValue(gender, out var honorific) ? honorific : string.Empty;
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
