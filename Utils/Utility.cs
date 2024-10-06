using OwlReadingRoom.Models.Enums;

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
                    return Color.FromArgb("#22C55E");
                case DeskStatus.NotAvailable:
                    return Color.FromArgb("#DF1212");
                case DeskStatus.Reserved:
                    return Color.FromArgb("#FACC15");
                case DeskStatus.Selected:
                    return Color.FromArgb("#BAD3FB");
                default:
                    return Color.FromArgb("#1E293B"); // Default color for unknown status
            }
        }

    }
}
