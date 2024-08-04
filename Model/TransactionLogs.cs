using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwlReadingRoom.Model
{
    public class TransactionLogs : BaseModel
    {
        [Column("transaction_id")]
        public int TransactionId { get; set; }

        [Column("last_paid_amount")]
        public double LastPaidAmount { get; set; }

        [Column("package_type_id")]
        public int PackageId { get; set; }

        [Column("payment_date")]
        public DateTime PaymentDate { get; set; }
    }
}
