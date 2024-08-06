using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwlReadingRoom.Models
{
    public class BaseModel
    {

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }

        [Column("created_by")]
        public string CreatedBy { get; set; }

        [Column("updated_by")]
        public string UpdatedBy { get; set; }
    }
}
