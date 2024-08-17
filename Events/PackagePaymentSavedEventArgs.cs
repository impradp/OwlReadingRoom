using OwlReadingRoom.ViewModels;

namespace OwlReadingRoom.Events
{
    public class PackagePaymentSavedEventArgs : EventArgs
    {
        public CustomerPackageViewModel SavedPersonalDetail { get; }
        public PackagePaymentSavedEventArgs(CustomerPackageViewModel savedPersonalDetail)
        {
            SavedPersonalDetail = savedPersonalDetail;
        }
    }
}
