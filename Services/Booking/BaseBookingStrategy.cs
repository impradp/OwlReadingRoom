using OwlReadingRoom.Models;
using OwlReadingRoom.Services.Resources;
using OwlReadingRoom.Services.Transactions;
using OwlReadingRoom.ViewModels;

namespace OwlReadingRoom.Services.Booking;

/// <summary>
/// An abstract base class that defines the common functionality for different booking strategies.
/// It provides a set of protected methods and properties that can be used by derived classes.
/// </summary>
public abstract class BaseBookingStrategy: IBookingStrategy
{
    protected readonly IBookingService _bookingService;
    protected readonly ITransactionService _transactionService;
    protected readonly IDeskService _deskService;

    protected BaseBookingStrategy(IBookingService bookingService, ITransactionService transactionService, IDeskService deskService)
    {
        _bookingService = bookingService;
        _transactionService = transactionService;
        _deskService = deskService;
    }

    /// <summary>
    /// Processes the booking based on the provided model and booking information.
    /// This is an abstract method that must be implemented by derived classes.
    /// </summary>
    /// <param name="model">The package and payment edit view model.</param>
    /// <param name="booking">The booking information.</param>
    public abstract void Process(PackageAndPaymentEditViewModel model, BookingInfo booking);

    /// <summary>
    /// Gets the supported type of the booking strategy.
    /// This is an abstract method that must be implemented by derived classes.
    /// </summary>
    /// <returns>The supported type of the booking strategy.</returns>
    public abstract string SupportedType();

    /// <summary>
    /// Saves the transaction log with the provided information.
    /// </summary>
    /// <param name="transactionId">The transaction ID.</param>
    /// <param name="paidAmount">The paid amount.</param>
    /// <param name="lastPaymentDate">The last payment date (optional).</param>
    /// <param name="paymentMethod">The payment method.</param>
    protected void SaveTransactionLog(int transactionId, double paidAmount, DateTime? lastPaymentDate, string paymentMethod)
    {
        TransactionLogs log = new TransactionLogs
        {
            LastPaidAmount = paidAmount,
            PaymentDate = lastPaymentDate ?? DateTime.Now,
            TransactionId = transactionId,
            PaymentMethod = paymentMethod
        };
        _transactionService.SaveTransactionLog(log);
    }

    /// <summary>
    /// Gets the payment status based on the due amount and the total amount.
    /// </summary>
    /// <param name="dueAmount">The due amount.</param>
    /// <param name="totalAmount">The total amount.</param>
    /// <returns>The payment status.</returns>
    protected PaymentStatusEnum GetPaymentStatus(decimal dueAmount, decimal totalAmount)
    {
        if (dueAmount == totalAmount)
            return PaymentStatusEnum.UNPAID;
        if (dueAmount == 0)
            return PaymentStatusEnum.PAID;
        if (dueAmount < 0)
            return PaymentStatusEnum.ADVANCED;
        return PaymentStatusEnum.PARTIALLY_PAID;
    }
}
