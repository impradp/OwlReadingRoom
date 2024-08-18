using OwlReadingRoom.DTOs;

namespace OwlReadingRoom.Services;

public interface IBookingService
{
    void RegisterWithMinimumDetails(MinimumCustomerDetail minimumCutomerDetail);

}
