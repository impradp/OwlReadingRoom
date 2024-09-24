using OwlReadingRoom.Models;
using OwlReadingRoom.ViewModels;

namespace OwlReadingRoom.Services.Booking;

public interface IBookingStrategy
{
    void Process(PackageAndPaymentEditViewModel model, BookingInfo booking);

    string SupportedType();
}
