using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwlReadingRoom.Models
{
    public class BookingInfo : BaseModel
    {
        [Column("package_id")]
        public int PackageId { get; set; }

        [Column("desk_id")]
        public int DeskId { get; set; }

        [Column("customer_id")]
        public int CustomerId { get; set; }

        [Column("transaction_id")]
        public int TransactionId { get; set; }
    }
}
