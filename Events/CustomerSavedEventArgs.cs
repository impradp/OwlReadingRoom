using OwlReadingRoom.ViewModels;

namespace OwlReadingRoom.Events
{
    public class CustomerSavedEventArgs : EventArgs
    {
        public CustomerPackageViewModel SavedCustomerPackage { get; }

        public CustomerSavedEventArgs(CustomerPackageViewModel customerPackage)
        {
            SavedCustomerPackage = customerPackage;
        }
    }
}
