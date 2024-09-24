using OwlReadingRoom.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwlReadingRoom.ViewModels
{
    public class PackageListViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public RoomType RoomType { get; set; }
        public double Price { get; set; }
        public int Days { get; set; }
        public bool IsSelectable { get; set; }
    }
}
