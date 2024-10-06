using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwlReadingRoom.Services.Startup
{
    public class WeeklyBackupStartupTask : IStartupTask
    {
        private readonly IStartupService _startupService;

        public WeeklyBackupStartupTask(IStartupService startupService)
        {
            _startupService = startupService;
        }

        public async Task ExecuteAsync()
        {
            await _startupService.InitiateWeeklyBackupAsync();
        }
    }
}
