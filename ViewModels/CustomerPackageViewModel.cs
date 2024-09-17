using OwlReadingRoom.Models;

namespace OwlReadingRoom.ViewModels
{
    public class CustomerPackageViewModel
    {
        public int CustomerId { get; set; }

        public int? BookingId { get; set; }
        public String FullName { get; set; }
        public String ContactNumber { get; set; }
        public String Faculty { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public String Package { get; set; }
        public String AllocatedSpace { get; set; }
        public String PaymentStatus { get; set; }
        public String Dues { get; set; }
        public Status Status { get; set; }
    }
}
