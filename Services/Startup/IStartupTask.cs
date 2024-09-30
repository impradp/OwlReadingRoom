using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwlReadingRoom.Services.Startup
{
    public interface IStartupTask
    {
        /// <summary>
        /// Executes the startup tasks when the application initiates for startup.
        /// </summary>
        /// <returns>The completed task for startup.</returns>
        Task ExecuteAsync();
    }
}
