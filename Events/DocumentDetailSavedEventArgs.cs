using OwlReadingRoom.ViewModels;

namespace OwlReadingRoom.Events
{
    public class DocumentDetailSavedEventArgs : EventArgs
    {
        public CustomerPackageViewModel SavedCustomerPackage { get; }

        public DocumentDetailSavedEventArgs(CustomerPackageViewModel customerPackage)
        {
            SavedCustomerPackage = customerPackage;
        }
    }
}
