using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwlReadingRoom.Services.Startup
{
    public interface IStartupService
    {
        /// <summary>
        /// Initiates the weekly backup of the database file and assets.
        /// Validates the previous backup taken and only takes backup on Friday once.
        /// </summary>
        /// <returns>The completed task of weekly backup process.</returns>
        Task InitiateWeeklyBackupAsync();

        /// <summary>
        /// Initiates the daily notification process for package expiration.
        /// This method will check for customers whose package will expire soon and notify them via email.
        /// The method also logs the notification status to prevent duplicate notifications on the same day.
        /// </summary>
        Task InitiateDailyReservationEndNotifyAsync();
    }
}
