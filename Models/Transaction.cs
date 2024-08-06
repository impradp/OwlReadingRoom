using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwlReadingRoom.Models
{
    public class Transaction : BaseModel
    {
        [Column("customer_id")]
        public int CustomerId { get; set; }

        [Column("paid_amount")]
        public double PaidAmount { get; set; }

        [Column("dues")]
        public double Dues { get; set; }

        [Column("payment_status")]
        public int PaymentStatusId { get; set; } // Foreign key to PaymentStatus
    }
}
