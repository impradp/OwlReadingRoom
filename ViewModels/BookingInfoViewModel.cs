using OwlReadingRoom.Models;

namespace OwlReadingRoom.ViewModels
{
    public class BookingInfoViewModel
    {
        public int Id { get; set; }
        public RoomType RoomType { get; set; }
        public String PackageName { get; set; }
        public String StartDate { get; set; }
        public String EndDate { get; set; }
    }
}
