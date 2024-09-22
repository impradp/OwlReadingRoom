using OwlReadingRoom.Models;

namespace OwlReadingRoom.ViewModels
{
    public class PackageAndPaymentEditViewModel
    {
        public int? Id { get; set; }
        public PackageListViewModel Package { get; set; }

        public RoomListViewModel Room { get; set; }

        public string DeskName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool HasLocker { get; set; }
        public bool HasParking { get; set; }

        public int? TransactionId { get; set; }
        public double PackageAmount { get; set; }
        public double LockerAmount { get; set; }
        public double ParkingAmount { get; set; }
        public double TotalAmount { get; set; }
        public double PaidAmount { get; set; }
        public double DueAmount { get; set; }
        public string PaymentStatus { get; set; }
        public DateTime? LastPaymentDate { get; set; }
    }
}
