namespace OwlReadingRoom.ViewModels
{
    public class CustomerPackageViewModel
    {
        public String FullName { get; set; }
        public String ContactNumber { get; set; }
        public String Faculty { get; set; }
        public String StartDate { get; set; }
        public String EndDate { get; set; }
        public String Package { get; set; }
        public String AllocatedSpace { get; set; }
        public String PaymentStatus { get; set; }
        public String Dues { get; set; }
        public DocumentViewModel Documents { get; set; }
        public TransactionDetailViewModel Transaction { get; set; }
    }
}
