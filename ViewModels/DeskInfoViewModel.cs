using OwlReadingRoom.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwlReadingRoom.ViewModels
{
    public class DeskInfoViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DeskStatus Status { get; set; }
        public object Color { get; set; }
        public object TextColor { get; set; }
        public string Message { get; set; }
    }
}
