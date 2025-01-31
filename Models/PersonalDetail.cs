﻿using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwlReadingRoom.Models
{
    public class PersonalDetail : BaseModel
    {
        [Column("full_name")]
        public string FullName { get; set; }

        [Column("dob")]
        public DateTime? DOB { get; set; }

        [Column("gender")]
        public string Gender { get; set; }

        [Column("faculty")]
        public string Faculty { get; set; }

        [Column("disease")]
        public string Disease { get; set; }

        [Column("allergies")]
        public string Allergies { get; set; }

        [Column("customer_id")]
        [Indexed]
        public int CustomerId { get; set; }
    }
}
