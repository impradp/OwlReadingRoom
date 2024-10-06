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

        [Column("mobile_number")]
        [Indexed(Unique = true)]
        public string MobileNumber { get; set; }

        [Ignore]
        public PersonalDetail PersonalDetail{ get; set; }

        [Ignore]
        public Address Address { get; set; }

        [Ignore]
        public List<DocumentInformation> Documents { get; set; }
    }
}
