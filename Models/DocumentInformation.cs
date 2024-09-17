using SQLite;

namespace OwlReadingRoom.Models
{
    public class DocumentInformation : BaseModel
    {
        [Column("document_number")]
        public string DocumentNumber { get; set; }

        [Column("issued_date")]
        public DateTime IssueDate { get; set; }

        [Column("document_type")]
        public DocumentType DocumentType { get; set; }

        [Column("customer_id")]
        [Indexed]
        public int CustomerId { get; set; }

        [Column("place_of_issue")]
        public string PlaceOfIssue{ get; set; }

        [Ignore]
        public List<DocumentImage> Images { get; set; }

    }

    public enum DocumentType
    {
        CITIZENSHIP, VOTERS_ID, LICENSE, PASSPORT
    }
}
