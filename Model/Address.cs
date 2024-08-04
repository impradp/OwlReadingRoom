using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwlReadingRoom.Model
{
    public class Address : BaseModel
    {
        [Column("province")]
        public string Province { get; set; }

        [Column("zone")]
        public string Zone { get; set; }

        [Column("metropolitan_city_vdc")]
        public string MetropolitanCity { get; set; }

        [Column("city")]
        public string City { get; set; }

        [Column("ward_no")]
        public int WardNo { get; set; }

        [Column("street")]
        public string Tole { get; set; }
    }
}
