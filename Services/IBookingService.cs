using OwlReadingRoom.DTOs;
using OwlReadingRoom.Models;
using OwlReadingRoom.ViewModels;
using static OwlReadingRoom.Services.BookingDetailsService;

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
    /// Retrieves a list of active customers along with their package information and related details.
    /// </summary>
    /// <remarks>
    /// This method performs the following steps:
    /// 1. Initializes queries for various entity types (Customer, PackageType, PersonalDetail, Room, Transaction, DocumentInformation, DocumentImage).
    /// 2. Groups bookings by customer and selects the latest booking for each customer.
    /// 3. Joins multiple tables to gather comprehensive information about active customers.
    /// 4. Filters the results to include only customers with active bookings (ReservationEndDate >= current date).
    /// 5. Constructs and returns a list of CustomerPackageViewModel objects containing detailed information about each active customer.
    /// </remarks>
    /// <returns>
    /// A List of CustomerPackageViewModel objects representing active customers. Each object includes:
    /// - FullName: The customer's full name
    /// - ContactNumber: The customer's mobile number
    /// - Faculty: The customer's faculty
    /// - StartDate: The start date of the customer's latest reservation
    /// - EndDate: The end date of the customer's latest reservation
    /// - Package: The name of the package associated with the latest booking
    /// - AllocatedSpace: The allocated room and desk for the customer
    /// - PaymentStatus: The payment status of the latest transaction (or "UNPAID" if no transaction exists)
    /// - Dues: The amount due for the latest transaction (or package price if no transaction exists)
    /// - Documents: A DocumentViewModel object containing information about the customer's documents (if any)
    /// </returns>
    /// <seealso cref="CustomerPackageViewModel"/>
    /// <seealso cref="DocumentViewModel"/>
    /// <seealso cref="DocumentImageViewModel"/>
    List<CustomerPackageViewModel> GetActiveCustomerList();

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

    /// <summary>
    /// Retrieves a list of inactive customers along with their last known package information and related details.
    /// </summary>
    /// <remarks>
    /// This method performs the following steps:
    /// 1. Initializes queries for various entity types (Customer, PackageType, PersonalDetail, Room, Transaction, DocumentInformation, DocumentImage).
    /// 2. Groups bookings by customer and selects the latest booking for each customer (if exists).
    /// 3. Performs a left join on multiple tables to gather comprehensive information about inactive customers.
    /// 4. Filters the results to include customers with no bookings or expired bookings (ReservationEndDate < current date).
    /// 5. Constructs and returns a list of CustomerPackageViewModel objects containing detailed information about each inactive customer.
    /// </remarks>
    /// <returns>
    /// A List of CustomerPackageViewModel objects representing inactive customers. Each object includes:
    /// - FullName: The customer's full name
    /// - ContactNumber: The customer's mobile number
    /// - Faculty: The customer's faculty
    /// - StartDate: The start date of the customer's latest reservation (or "N/A" if no booking exists)
    /// - EndDate: The end date of the customer's latest reservation (or "N/A" if no booking exists)
    /// - Package: The name of the package associated with the latest booking (or "N/A" if no booking exists)
    /// - AllocatedSpace: The last allocated room and desk for the customer (or "N/A" if no booking exists)
    /// - PaymentStatus: The payment status of the latest transaction (or "UNPAID" if no transaction exists)
    /// - Dues: The amount due for the latest transaction (or package price if no transaction exists, or "N/A")
    /// - Documents: A DocumentViewModel object containing information about the customer's documents (if any)
    /// </returns>
    /// <seealso cref="CustomerPackageViewModel"/>
    /// <seealso cref="DocumentViewModel"/>
    /// <seealso cref="DocumentImageViewModel"/>
    List<CustomerPackageViewModel> GetInactiveCustomerList();

    /// <summary>
    /// Retrieves booking details for a given booking ID.
    /// </summary>
    /// <param name="bookingId">The ID of the booking.</param>
    /// <returns>A BookingInfoViewModel containing booking details, or null if the bookingId is not provided.</returns>
    BookingInfoViewModel GetBookingDetails(int? bookingId);

    /// <summary>
    /// Retrieves the booking details by the booking ID.
    /// </summary>
    /// <param name="bookingId">The ID of the booking to retrieve.</param>
    /// <returns>The booking information with the specified ID.</returns>
    BookingInfo GetBookingDetailsById(int? bookingId);

    /// <summary>
    /// Checks if there are any active bookings for a given package.
    /// </summary>
    /// <param name="packageId">The ID of the package to check.</param>
    /// <returns>True if there are active bookings for the package, false otherwise.</returns>
    bool ContainsActiveBookingWithPackageId(int? packageId);

    /// <summary>
    /// Performs the initial booking process.
    /// This method registers a new customer with the minimum required details and creates a new booking.
    /// </summary>
    /// <param name="minimumCustomerDetail">The minimum customer details required for booking.</param>
    void PerformInitialBooking(MinimumCustomerDetail minimumCustomerDetail);

    /// <summary>
    /// Performs the initial booking process.
    /// This method registers a new customer with the minimum required details and creates a new booking.
    /// </summary>
    /// <param name="bookingInfo">The to-be created booking info of the current customer.</param>
    void SaveItem(BookingInfo bookingInfo);

    /// <summary>
    /// Renews the customer package association with new booking id.
    /// </summary>
    /// <param name="customerId">The unique identifier of customer.</param>
    /// <returns>The created booking info id</returns>
    int PerformCustomerRenew(int customerId);
}
