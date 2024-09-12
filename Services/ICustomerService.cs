using OwlReadingRoom.DTOs;
using OwlReadingRoom.Models;
using SQLite;

namespace OwlReadingRoom.Services;

public interface ICustomerService
{
    /// <summary>
    /// Creates a new customer record with the provided minimum details.
    /// </summary>
    /// <param name="minimumCustomerDetail">An object containing the minimum required details for customer creation.</param>
    /// <returns>A newly created Customer object with associated personal details and temporary address.</returns>
    /// <remarks>
    /// This method performs the following steps:
    /// 1. Creates a new Customer object with the provided contact number.
    /// 2. Saves the new Customer object to the repository.
    /// 3. Creates and associates a PersonalDetail object with the customer.
    /// 4. Creates and associates a temporary Address object with the customer.
    /// 5. Returns the fully created and associated Customer object.
    /// </remarks>
    /// <seealso cref="MinimumCustomerDetail"/>
    /// <seealso cref="Customer"/>
    /// <seealso cref="PersonalDetail"/>
    /// <seealso cref="Address"/>
    Customer CreateNewCustomerWithMinimumDetails(MinimumCustomerDetail minimumCutomerDetail);

    /// <summary>
    /// Retrieves a customer from the repository based on their mobile number.
    /// </summary>
    /// <param name="mobileNumber">The mobile number of the customer to retrieve.</param>
    /// <returns>
    /// A Customer object if a customer with the specified mobile number is found;
    /// otherwise, null.
    /// </returns>
    /// <remarks>
    /// This method performs a case-sensitive comparison on the mobile number.
    /// It uses LINQ to query the customer repository and returns the first matching customer,
    /// or null if no match is found.
    /// </remarks>
    /// <seealso cref="Customer"/>
    Customer GetCustomerByMobileNumber(string mobileNumber);

    TableQuery<PersonalDetail> PersonalDetailTableQuery { get; }

    TableQuery<DocumentInformation> DocumentTableQuery { get; }
    TableQuery<DocumentImage> DocumentImageTableQuery { get; }

    TableQuery<Address> AddressTableQuery { get; }
    TableQuery<Customer> CustomerTableQuery { get; }
}
