
namespace OwlReadingRoom.Services.Startup;

public class NotifyReservationExpiryTask : IStartupTask
{
    private readonly IStartupService _startupService;

    public NotifyReservationExpiryTask(IStartupService startupService)
    {
        _startupService = startupService;
    }

    public async Task ExecuteAsync()
    {
        await _startupService.InitiateDailyReservationEndNotifyAsync();
    }
}
