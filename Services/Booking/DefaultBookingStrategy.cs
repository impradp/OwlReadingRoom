using OwlReadingRoom.Models;
using OwlReadingRoom.Services.Resources;
using OwlReadingRoom.Services.Transactions;
using OwlReadingRoom.ViewModels;

namespace OwlReadingRoom.Services.Booking;

/// <summary>
/// The `DefaultBookingStrategy` class is responsible for processing default booking requests; Requests for changes in rooms/desks/amount.
/// </summary>
public class DefaultBookingStrategy : BaseBookingStrategy
{
    public DefaultBookingStrategy(IBookingService bookingService, ITransactionService transactionService, IDeskService deskService) : base(bookingService, transactionService, deskService)
    {
    }

    /// <summary>
    /// Processes the booking based on the provided view model and booking information.
    /// This method is responsible for updating the existing transaction with the new payment information and saving the transaction log if needed.
    /// </summary>
    /// <param name="packagePaymentDetail">The package and payment edit view model, which contains the updated payment information.</param>
    /// <param name="bookingInfo">The booking information, which cont
    public override void Process(PackageAndPaymentEditViewModel packagePaymentDetail, BookingInfo bookingInfo)
    {
        UpdateBookingInformation(bookingInfo, packagePaymentDetail);

        if (packagePaymentDetail.PaidAmount > 0)
        {
            Transaction transaction = _transactionService.TableQuery.FirstOrDefault(t => t.BookingInformationId == bookingInfo.Id);

            transaction.DueAmount = decimal.ToDouble(CalculateDueAmount(transaction.DueAmount, packagePaymentDetail.PaidAmount));
            transaction.PaidAmount = decimal.ToDouble(CalculatePaidAmount(transaction.PaidAmount, packagePaymentDetail.PaidAmount));
            transaction.PaymentStatus = GetPaymentStatus((decimal)transaction.DueAmount, (decimal)transaction.TotalAmount);
            _transactionService.SaveTransaction(transaction);

            SaveTransactionLog(transaction.Id, packagePaymentDetail.PaidAmount, packagePaymentDetail.LastPaymentDate, packagePaymentDetail.PaymentMethod);
        }
    }

    /// <summary>
    /// Updates the desk information when the customer wants to switch the desk.
    /// </summary>
    /// <param name="bookingInfo">The booking information of the current package for the definite customer.</param>
    /// <param name="packagePaymentDetail">The current package and payment information of the dedicated customer.</param>
    private void UpdateBookingInformation(BookingInfo bookingInfo, PackageAndPaymentEditViewModel packagePaymentDetail)
    {
        BookingInfo bookingDetails = _bookingService.GetBookingDetailsById(bookingInfo?.Id);

        Desk desk = _deskService.TableQuery.FirstOrDefault(d => d.RoomId == packagePaymentDetail.Room.Id && d.Name.Equals(packagePaymentDetail.DeskName));

        bookingDetails.DeskId = desk.Id;
        _bookingService.SaveItem(bookingDetails);
    }

    /// <summary>
    /// Calculates the due amount based on the previous due amount and the current paid amount.
    /// </summary>
    /// <param name="previousDue">The previous due amount.</param>
    /// <param name="currentPaidAmount">The current paid amount.</param>
    /// <returns>The calculated due amount.</returns>
    public decimal CalculateDueAmount(double previousDue, double currentPaidAmount)
    {
        decimal dueAmount = (decimal)previousDue - (decimal)currentPaidAmount;
        return Math.Round(dueAmount, 2, MidpointRounding.ToEven);
    }

    /// <summary>
    /// Calculates the paid amount based on the previous paid amount and the current paid amount.
    /// </summary>
    /// <param name="previousPaid">The previous paid amount.</param>
    /// <param name="currentPaidAmount">The current paid amount.</param>
    /// <returns>The calculated paid amount.</returns>
    public decimal CalculatePaidAmount(double previousPaid, double currentPaidAmount)
    {
        decimal paidAmount = (decimal)previousPaid + (decimal)currentPaidAmount;
        return Math.Round(paidAmount, 2, MidpointRounding.ToEven);
    }

    public override string SupportedType()
    {
        return BookingStrategyNames.Default;
    }
}
