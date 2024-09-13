using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwlReadingRoom.Utils
{
    public class RoomPlan
    {
        public static IEnumerable<string> Non_AC_Room_Desks = new List<string> { "N1", "N2", "N3", "N4","N5","N6","N7","M1","M2", "M3", "M4", "M5", "M6", "M7", "M8", "M9", "M10", "M11", "M12", "A1", "A2", "A3", "A4", "A5", "A6", "A7", "A8", "A9", "A10", "A11" };
        public static IEnumerable<string> AC_Room_Desks = new List<string> { };
    }
}
