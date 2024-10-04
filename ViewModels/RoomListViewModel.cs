using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwlReadingRoom.ViewModels
{
    public class RoomListViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string RoomType { get; set; }
        public int TotalDesks { get; set; }
        public int AvailableDesks { get; set; }
        public List<DeskInfoViewModel> Desks { get; set; }
        public int DeskCount { get; set; }
        public bool IsSelectable { get; set; }
        public string SelectedDesk { get; set; }
    }
}
