using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace OwlReadingRoom.Models
{
    public class Document : BaseModel
    {
        [Column("document_number")]
        public string DocumentNumber { get; set; }

        [Column("issued_date")]
        public DateTime IssueDate { get; set; }

        [Column("document_type")]
        public DocumentType DocumentType { get; set; }

        [Column("location")]
        public string Location { get; set; }

    }

    public enum DocumentType
    {
        CTZN, VOTERS_ID, LICENSE
    }
}
