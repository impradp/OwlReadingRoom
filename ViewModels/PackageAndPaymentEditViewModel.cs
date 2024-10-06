using OwlReadingRoom.Models;

namespace OwlReadingRoom.ViewModels
{
    public class PackageAndPaymentEditViewModel
    {
        public int? Id { get; set; }
        public PackageListViewModel Package { get; set; }

        public RoomListViewModel Room { get; set; }

        public string DeskName { get; set; }
        public DateTime? PackageStartDate { get; set; } = DateTime.Now;
        public DateTime? PackageEndDate { get; set; }
        public int? TransactionId { get; set; }
        public double PackageAmount { get; set; }
        public double LockerAmount { get; set; }
        public double ParkingAmount { get; set; }
        public double TotalAmount { get; set; }
        public double PaidAmount { get; set; }
        public double DueAmount { get; set; }
        public string PaymentStatus { get; set; }
        public DateTime? LastPaymentDate { get; set; } = DateTime.Now;
        public string PaymentMethod { get; set; }
    }
}
