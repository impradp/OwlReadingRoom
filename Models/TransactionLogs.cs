using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwlReadingRoom.Models
{
    public class TransactionLogs : BaseModel
    {
        [Column("transaction_id")]
        public int TransactionId { get; set; }

        [Column("last_paid_amount")]
        public double LastPaidAmount { get; set; }

        [Column("payment_method")]
        public string PaymentMethod { get; set; }

        [Column("payment_date")]
        public DateTime PaymentDate { get; set; }
    }
}
