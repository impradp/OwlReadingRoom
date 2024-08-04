using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwlReadingRoom.Model
{
    public class PaymentStatus : BaseModel
    {
        [Column("status")]
        public string Status { get; set; }
    }    
}
