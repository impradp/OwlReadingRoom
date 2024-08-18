using SQLite;

namespace OwlReadingRoom.Models
{
    public class PaymentStatus : BaseModel
    {
        [Column("status")]
        public string Status { get; set; }
    }
}
