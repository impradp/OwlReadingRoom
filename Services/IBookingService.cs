using OwlReadingRoom.DTOs;
using OwlReadingRoom.ViewModels;
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
    /// Registers a new customer with minimum details, ensuring no duplicate entries exist.
    /// </summary>
    /// <param name="minimumCustomerDetail">An object containing the minimum required details for customer registration.</param>
    /// <remarks>
    /// This method performs the following steps:
    /// 1. Checks if a customer with the provided contact number already exists.
    /// 2. If a customer exists, it throws an InvalidDataException.
    /// 3. If no existing customer is found, it creates a new customer with the provided minimum details.
    /// 
    /// This method is marked with the [Transactional] attribute, ensuring that all database operations
    /// within the method are executed as a single transaction. If any part of the operation fails,
    /// all changes will be rolled back.
    /// </remarks>
    /// <exception cref="InvalidDataException">
    /// Thrown when a customer with the provided contact number already exists in the system.
    /// </exception>
    /// <seealso cref="MinimumCustomerDetail"/>
    /// <seealso cref="CreateNewCustomerWithMinimumDetails"/>
    void RegisterWithMinimumDetails(MinimumCustomerDetail minimumCutomerDetail);

}
