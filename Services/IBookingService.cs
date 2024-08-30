using OwlReadingRoom.DTOs;

namespace OwlReadingRoom.Services;

public interface IBookingService
{
    /// <summary>
    /// Fetches the count of unavailable desks for each room at the current time.
    /// </summary>
    /// <returns>
    /// A dictionary where the key is the room ID (int) and the value is the count of unavailable desks (int).
    /// </returns>
    /// <remarks>
    /// This method performs a join operation between desks and bookings to determine
    /// which desks are currently unavailable. A desk is considered unavailable if there's
    /// an active booking for it at the current time.
    /// </remarks>
    Dictionary<int, int> FetchUnavailableDesksCount();
    void RegisterWithMinimumDetails(MinimumCustomerDetail minimumCutomerDetail);

}
