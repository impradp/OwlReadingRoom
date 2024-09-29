using OwlReadingRoom.Models;
using OwlReadingRoom.Proxy;
using OwlReadingRoom.Services.Constants;
using OwlReadingRoom.Services.Resources;
using OwlReadingRoom.Services.Transactions;
using OwlReadingRoom.ViewModels;

namespace OwlReadingRoom.Services.Booking;

/// <summary>
/// The `NewPackageBookingStrategy` class is responsible for processing booking requests for new packages.
/// This strategy is used when a new package is assigned to a customer
/// </summary>
public class NewPackageBookingStrategy : BaseBookingStrategy
{
    public NewPackageBookingStrategy(IBookingService bookingService, ITransactionService transactionService, IDeskService deskService) : base(bookingService, transactionService, deskService)
    {
    }

    /// <summary>
    /// Processes the booking based on the provided view model and booking information.
    /// This method is responsible for updating the booking information, calculating the total amount and due amount, creating a new transaction, and saving the transaction log if needed.
    /// </summary>
    /// <param name="packagePaymentDetail">The package and payment edit view model, which contains the details of the new package and payment information.</param>
    /// <param name="bookingInfo">The booking information, which contains the details of the existing booking.</param>
    [Transactional]
    public override void Process(PackageAndPaymentEditViewModel packagePaymentDetail, BookingInfo bookingInfo)
    {
        // Update Booking Information
        UpdateBookingInformation(bookingInfo, packagePaymentDetail);

        // Calculate total amount and due amount
        decimal totalAmount = CalculateTotalAmount(packagePaymentDetail);
        decimal dueAmount = CalculateDueAmount(totalAmount, (decimal)packagePaymentDetail.PaidAmount);

        // Create and save the transaction
        Transaction transaction = CreateTransaction(bookingInfo.Id, totalAmount, dueAmount, packagePaymentDetail.PaidAmount, packagePaymentDetail.LockerAmount, packagePaymentDetail.ParkingAmount);
        _transactionService.SaveTransaction(transaction);

        // Create and save transaction log if needed
        if (packagePaymentDetail.PaidAmount > 0)
        {
            SaveTransactionLog(transaction.Id, packagePaymentDetail.PaidAmount, packagePaymentDetail.LastPaymentDate, packagePaymentDetail.PaymentMethod);
        }

    }

    /// <summary>
    /// Updates the booking information based on the provided view model.
    /// </summary>
    /// <param name="bookingInfo">The booking information to be updated.</param>
    /// <param name="packagePaymentDetail">The package and payment edit view model, which contains the updated package details.</param>
    private void UpdateBookingInformation(BookingInfo bookingInfo, PackageAndPaymentEditViewModel packagePaymentDetail)
    {
        bookingInfo.PackageId = packagePaymentDetail.Package.Id;
        // Set ReservationStartDate to 12:00 AM (start of the day)
        bookingInfo.ReservationStartDate = packagePaymentDetail.PackageStartDate?.Date;
        // Set ReservationEndDate to 11:59 PM (end of the day)
        bookingInfo.ReservationEndDate = packagePaymentDetail.PackageEndDate?.Date.AddDays(1).AddSeconds(-1) ?? DateTime.MinValue;

        Desk desk = _deskService.TableQuery.FirstOrDefault(d => d.RoomId == packagePaymentDetail.Room.Id && d.Name.Equals(packagePaymentDetail.DeskName));

        bookingInfo.DeskId = desk.Id;
        _bookingService.SaveItem(bookingInfo);
    }

    /// <summary>
    /// Calculates the total amount based on the provided view model.
    /// The total amount includes the package amount, locker fee, and parking fee.
    /// </summary>
    /// <param name="packagePaymentDetail">The package and payment edit view model, which contains the package details.</param>
    /// <returns>The calculated total amount.</returns>
    private decimal CalculateTotalAmount(PackageAndPaymentEditViewModel packagePaymentDetail)
    {
        decimal packageAmount = (decimal)packagePaymentDetail.Package.Price;

        decimal lockerAmount = (decimal)packagePaymentDetail.LockerAmount;
        decimal parkingAmount = (decimal)packagePaymentDetail.ParkingAmount;

        // Calculate and round total amount
        decimal totalAmount = packageAmount + lockerAmount + parkingAmount;
        return Math.Round(totalAmount, 2, MidpointRounding.ToEven);
    }

    /// <summary>
    /// Calculates the due amount based on the total amount and the paid amount.
    /// </summary>
    /// <param name="totalAmount">The total amount.</param>
    /// <param name="paidAmount">The paid amount.</param>
    /// <returns>The calculated due amount.</returns>
    private decimal CalculateDueAmount(decimal totalAmount, decimal paidAmount)
    {
        decimal dueAmount = totalAmount - (decimal)paidAmount;
        return Math.Round(dueAmount, 2, MidpointRounding.ToEven);
    }

    /// <summary>
    /// Creates a new transaction based on the provided information.
    /// </summary>
    /// <param name="bookingInfoId">The booking information ID.</param>
    /// <param name="totalAmount">The total amount.</param>
    /// <param name="dueAmount">The due amount.</param>
    /// <param name="paidAmount">The paid amount.</param>
    /// <returns>The new transaction.</returns>
    private Transaction CreateTransaction(int bookingInfoId, decimal totalAmount, decimal dueAmount, double paidAmount, double lockerAmount, double parkingAmount)
    {
        return new Transaction
        {
            BookingInformationId = bookingInfoId,
            PaymentStatus = GetPaymentStatus(dueAmount, totalAmount),
            DueAmount = decimal.ToDouble(dueAmount),
            TotalAmount = decimal.ToDouble(totalAmount),
            PaidAmount = paidAmount,
            LockerAmount = lockerAmount,
            ParkingAmount = parkingAmount,
        };
    }

    public override string SupportedType()
    {
        return BookingStrategyNames.NewPackage;
    }
}
