namespace OwlReadingRoom.ViewModels
{
    public class TransactionDetailViewModel
    {
        public int TransactionId { get; set; }
        public double PackageAmount { get; set; }
        public double LockerAmount { get; set; }
        public double ParkingAmount { get; set; }
        public double TotalAmount { get; set; }
        public double PaidAmount { get; set; }
        public double DueAmount { get; set; }
    }
}
