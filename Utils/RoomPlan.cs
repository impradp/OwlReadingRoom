using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwlReadingRoom.Utils
{
    public class RoomPlan
    {
        public static IEnumerable<string> Non_AC_Room_Desks = new List<string> { "N1", "N2", "N3", "N4", "N5", "N6", "N7", "M1", "M2", "M3", "M4", "M5", "M6", "M7", "M8", "M9", "M10", "M11", "M12", "A1", "A2", "A3", "A4", "A5", "A6", "A7", "A8", "A9", "A10", "A11" };
        public static IEnumerable<string> AC_Room_Desks = new List<string> { "A1", "A2", "A3", "A4", "A5", "A6", "A7", "A8", "A9", "A10", "A11", "A12", "A13", "A14", "A15", "B1", "B2", "B3", "B4", "B5", "B6", "B7", "B8", "B9", "B10", "B11", "B12", "B13", "B14", "B15", "B16", "B17", "B18", "B19", "B20" };
    }
}
