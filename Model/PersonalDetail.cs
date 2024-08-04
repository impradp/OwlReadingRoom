using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwlReadingRoom.Model
{
    public class PersonalDetail : BaseModel
    {
        [Column("full_name")]
        public string FullName { get; set; }

        [Column("dob")]
        public DateTime DOB { get; set; }

        [Column("gender")]
        public string Gender { get; set; }
    }
}
