using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwlReadingRoom.Model
{
    public class Customer : BaseModel
    {
        [Column("first_name")]
        public string FirstName { get; set; }

        [Column("last_name")]
        public string LastName { get; set; }

        [Column("mobile_number")]
        public double MobileNumber { get; set; }

        [Column("faculty")]
        public string Faculty { get; set; }

        [Column("date_of_birth")]
        public DateTime DateOfBirth { get; set; }

        [Column("last_active")]
        public DateTime LastActive { get; set; } 
    }
}
