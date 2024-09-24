using OwlReadingRoom.ViewModels;

namespace OwlReadingRoom.Services.Booking;

public interface IBookingProcessor
{
    void ProcessBooking(PackageAndPaymentEditViewModel model);
}
