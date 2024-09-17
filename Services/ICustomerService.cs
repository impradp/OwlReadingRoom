using OwlReadingRoom.DTOs;
using OwlReadingRoom.Models;
using OwlReadingRoom.ViewModels;
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

    /// <summary>
    /// Retrieves document details for a given customer ID.
    /// </summary>
    /// <param name="customerId">The ID of the customer.</param>
    /// <returns>A DocumentViewModel containing the customer's document information and associated images.</returns>
    DocumentViewModel GetCustomerDocumentDetails(int customerId);

    /// <summary>
    /// Retrieves personal details for a given customer ID.
    /// </summary>
    /// <param name="customerId">The ID of the customer.</param>
    /// <returns>A PersonalDetail object containing the customer's personal information.</returns>
    PersonalDetail GetPersonalDetails(int customerId);

    /// <summary>
    /// Retrieves address details for a given customer ID.
    /// </summary>
    /// <param name="customerId">The ID of the customer.</param>
    /// <returns>An Address Model containing the customer's temporary address and permanent address.</returns>
    Address GetAddressDetails(int customerId);  
    TableQuery<PersonalDetail> PersonalDetailTableQuery { get; }

    TableQuery<DocumentInformation> DocumentTableQuery { get; }
    TableQuery<DocumentImage> DocumentImageTableQuery { get; }

    TableQuery<Address> AddressTableQuery { get; }
    TableQuery<Customer> CustomerTableQuery { get; }
}
