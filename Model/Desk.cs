using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwlReadingRoom.Model
{
    public class Desk : BaseModel
    {
        [Column("room_id")]
        public int RoomId { get; set; }

        [Column("name")]
        public string Name { get; set; }
    }
}
