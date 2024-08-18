using OwlReadingRoom.DTOs;

namespace OwlReadingRoom.Services;

public interface ICustomerService
{
    /**
     * save customer with minimum details: full name, document location, mobile number 
     */
    int CreateNewCustomerWithMinimumDetails(MinimumCustomerDetail minimumCutomerDetail);
}
