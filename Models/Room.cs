using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwlReadingRoom.Models
{
    public class Room : BaseModel
    {
        [Column("flat_id")]
        public int FlatId { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("room_type")]
        public RoomType RoomType { get; set; }
    }

    public enum RoomType
    {
        AC,
        NON_AC
    }
}
