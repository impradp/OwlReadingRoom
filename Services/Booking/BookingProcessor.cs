using Microsoft.Extensions.DependencyInjection;
using OwlReadingRoom.Models;
using OwlReadingRoom.ViewModels;

namespace OwlReadingRoom.Services.Booking;

/// <summary>
/// The `BookingProcessor` class is responsible for processing booking requests based on the appropriate booking strategy.
/// </summary>
public class BookingProcessor : IBookingProcessor
{
    private readonly IDictionary<string, IBookingStrategy> _bookingStrategies;
    private readonly IBookingService _bookingService;

    public BookingProcessor(IEnumerable<IBookingStrategy> bookingStrategies,
                            IBookingService bookingService)
    {
        _bookingStrategies = bookingStrategies.ToDictionary(n => n.SupportedType(), n => n);
        _bookingService = bookingService;
    }

    /// <summary>
    /// Processes the booking based on the provided view model.
    /// </summary>
    /// <param name="model">The package and payment edit view model.</param>
    public void ProcessBooking(PackageAndPaymentEditViewModel model)
    {
        BookingInfo booking = _bookingService.GetBookingDetailsById(model.Id);
        if (booking.PackageId == null)
        {
            GetBookingStrategy(BookingStrategyNames.NewPackage).Process(model, booking);
        }
        else if (model.Package?.Id != booking.PackageId)
        {
            GetBookingStrategy(BookingStrategyNames.PackageChange).Process(model, booking);
        }
        else
        {
            GetBookingStrategy(BookingStrategyNames.Default).Process(model, booking);
        }
    }

    /// <summary>
    /// Gets the booking strategy with the specified name.
    /// </summary>
    /// <param name="name">The name of the booking strategy.</param>
    /// <returns>The booking strategy instance, or throws an `ArgumentException` if the name is not found.</returns>
    public IBookingStrategy GetBookingStrategy(string name)
    {
        if (_bookingStrategies.TryGetValue(name, out var client))
            return client;

        // handle error
        throw new ArgumentException(nameof(name));
    }
}
