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

        [Column("total_amount")]
        public double TotalAmount { get; set; }

        [Column("paid_amount")]
        public double PaidAmount { get; set; }

        [Column("due_amount")]
        public double DueAmount { get; set; }

        [Column("booking_information_id")]
        public int BookingInformationId { get; set; }

        [Column("payment_status")]
        public PaymentStatusEnum PaymentStatus { get; set; } // Foreign key to PaymentStatus
    }

    public enum PaymentStatusEnum
    {
        PAID, UNPAID, PARTIALLY_PAID, ADVANCED
    }
}
