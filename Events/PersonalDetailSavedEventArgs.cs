using OwlReadingRoom.ViewModels;

namespace OwlReadingRoom.Events
{
    public class PersonalDetailSavedEventArgs : EventArgs
    {
        public CustomerPackageViewModel SavedPersonalDetail { get; }
        public PersonalDetailSavedEventArgs(CustomerPackageViewModel savedPersonalDetail)
        {
            SavedPersonalDetail = savedPersonalDetail;
        }
    }
}
