using OwlReadingRoom.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwlReadingRoom.Utils
{
    public static class Utility
    {
        public static string GetTimeOfDay()
        {
            var hour = DateTime.Now.Hour;
            if (hour < 12) return "Morning";
            if (hour < 18) return "Afternoon";
            return "Evening";
        }

        public static object GetBackGroundColorByDeskStatus(DeskStatus status)
        {
            switch (status)
            {
                case DeskStatus.Available:
                    return Color.FromHex("#E7F7E5");
                case DeskStatus.NotAvailable:
                    return Color.FromHex("#FFE5E5");
                case DeskStatus.Reserved:
                    return Color.FromHex("#FCDDAE");
                default:
                    return Color.FromHex("#FFFFFF"); // Default color for unknown status
            }
        }

        public static object GetTextColorByDeskStatus(DeskStatus status)
        {
            switch (status)
            {
                case DeskStatus.Available:
                    return Color.FromHex("#29BB00");
                case DeskStatus.NotAvailable:
                    return Color.FromHex("#B91C1C");
                case DeskStatus.Reserved:
                    return Color.FromHex("#F59E0B");
                default:
                    return Color.FromHex("#FFFFFF"); // Default color for unknown status
            }
        }
    }
}
