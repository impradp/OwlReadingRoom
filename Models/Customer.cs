using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwlReadingRoom.Models
{
    public class Customer : BaseModel
    {
        [Column("personal_detail_id")]
        public int PersonalDetailId { get; set; }

        [Column("permanent_address_id")]
        public int PermanentAddressId { get; set; }

        [Column("temporary_address_id")]
        public int TemporaryAddressId { get; set; }

        [Column("document_id")]
        public int DocumentId { get; set; }

        [Column("mobile_number")]
        public string MobileNumber { get; set; }
    }
}
