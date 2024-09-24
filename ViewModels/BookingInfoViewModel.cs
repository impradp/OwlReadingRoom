using OwlReadingRoom.Models;

namespace OwlReadingRoom.ViewModels
{
    public class BookingInfoViewModel
    {
        public int? Id { get; set; }
        public RoomType? RoomType { get; set; }
        public string PackageName { get; set; }
        public double PackagePrice { get; set; }
        public string DeskName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool HasLocker { get; set; }
        public bool HasParking { get; set; }
    }
}
