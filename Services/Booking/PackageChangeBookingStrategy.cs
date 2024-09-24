using OwlReadingRoom.Models;
using OwlReadingRoom.Services.Resources;
using OwlReadingRoom.Services.Transactions;
using OwlReadingRoom.ViewModels;

namespace OwlReadingRoom.Services.Booking;

public class PackageChangeBookingStrategy : BaseBookingStrategy
{
    public PackageChangeBookingStrategy(IBookingService bookingService, ITransactionService transactionService, IDeskService deskService) : base(bookingService, transactionService, deskService)
    {
    }

    public override void Process(PackageAndPaymentEditViewModel packagePaymentDetail, BookingInfo booking)
    {
        
    }
   
    public override string SupportedType()
    {
        return BookingStrategyNames.PackageChange;
    }
}
