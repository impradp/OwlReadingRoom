using OwlReadingRoom.DTOs;
using static OwlReadingRoom.Services.BookingService;

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

    /// <summary>
    /// Retrieves a dictionary containing desk booking information for a specific room.
    /// </summary>
    /// <param name="roomId">The ID of the room for which to retrieve booking information.</param>
    /// <returns>
    /// A dictionary where the key is the desk ID, and the value is a list of <see cref="ReservationInfo"/> 
    /// objects containing information about reservations for that desk.
    /// </returns>
    /// <remarks>
    /// The method performs a query that joins the desks with their corresponding bookings, 
    /// customer details, and personal details, filtered by the specified room ID and the current 
    /// time to only include active reservations. The results are grouped by desk ID and returned as 
    /// a dictionary.
    /// </remarks>
    Dictionary<int, List<ReservationInfo>> GetDeskBookingInformation(int roomId);
    void RegisterWithMinimumDetails(MinimumCustomerDetail minimumCutomerDetail);

}
