namespace OwlReadingRoom.ViewModels
{
    public class DocumentImageViewModel
    {
        public int? Id { get; set; }
        public string ImagePath { get; set; }

        public string ImageName { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
