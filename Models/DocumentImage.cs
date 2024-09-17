using SQLite;

namespace OwlReadingRoom.Models;

public class DocumentImage : BaseModel
{
    [Indexed]
    [Column("document_information_id")]
    public int DocumentInformationId { get; set; }
    [Column("image_path")]
    public string ImagePath { get; set; }

    [Column("status")]
    public Status status { get; set; }
}
