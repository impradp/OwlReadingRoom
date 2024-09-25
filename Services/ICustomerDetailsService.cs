
using OwlReadingRoom.ViewModels;

namespace OwlReadingRoom.Services
{
    public interface ICustomerDetailsService
    {
        /// <summary>
        /// Fetches detailed customer information based on minimal customer package information.
        /// </summary>
        /// <param name="minimumInformation">A CustomerPackageViewModel containing minimal customer information.</param>
        /// <returns>A CustomerDetailViewModel containing comprehensive customer details.</returns>
        CustomerDetailViewModel fetchCustomerDetails(CustomerPackageViewModel minimumInformation);


        /// <summary>
        /// Renews the booking for inactive customers.
        /// </summary>
        /// <param name="customerDetailViewModel">The customer details to be updated with new booking id.</param>
        void RenewCustomerBooking(CustomerDetailViewModel customerDetailViewModel);
    }
}
