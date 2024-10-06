using OwlReadingRoom.ViewModels;

namespace OwlReadingRoom.Events
{
    public class CustomerSavedEventArgs : EventArgs
    {
        public CustomerDetailViewModel SavedCustomerDetail { get; }

        public CustomerSavedEventArgs(CustomerDetailViewModel customerDetail)
        {
            SavedCustomerDetail = customerDetail;
        }
    }
}
