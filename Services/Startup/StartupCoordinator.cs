using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwlReadingRoom.Services.Startup
{
    public class StartupCoordinator
    {
        private readonly IEnumerable<IStartupTask> _startupTasks;

        public StartupCoordinator(IEnumerable<IStartupTask> startupTasks)
        {
            _startupTasks = startupTasks;
        }

        public async Task ExecuteStartupTasksAsync()
        {
            foreach (var task in _startupTasks)
            {
                await task.ExecuteAsync();
            }
        }
    }
}
