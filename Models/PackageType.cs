using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwlReadingRoom.Models
{
    public class PackageType : BaseModel
    {
        [Column("name")]
        public string Name { get; set; }

        [Column("price")]
        public double Price { get; set; }

        [Column("room_type")]
        public RoomType RoomType { get; set; }

        [Column("enabled")]
        public bool Enabled { get; set; } = true;

        [Column("days")]
        public int Days { get; set; }
    }
}
