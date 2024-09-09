using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwlReadingRoom.Models
{
    public class Address : BaseModel
    {
        [Column("temporary_address")]
        public string TemporaryAddress { get; set; }
        [Column("permanent_address")]
        public string PermanentAddress { get; set; }

        [Column("customer_id")]
        [Indexed]
        public int CustomerId { get; set; }
    }
}
