using SQLite;

namespace OwlReadingRoom.Models
{
    public class BookingInfo : BaseModel
    {
        [Column("package_id")]
        public int? PackageId { get; set; }

        [Column("desk_id")]
        public int? DeskId { get; set; }

        [Column("customer_id")]
        public int CustomerId { get; set; }

        [Column("reservation_start_date")]
        public DateTime? ReservationStartDate { get; set; }

        [Column("reservation_end_date")]
        public DateTime? ReservationEndDate { get; set; }

        [Column("locker_facility")]
        public bool HasBookedLocker { get; set; }

        [Column("parking_facility")]
        public bool HasBookedParking { get; set; }
    }
}
