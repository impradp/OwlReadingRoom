using OwlReadingRoom.Models;
using OwlReadingRoom.ViewModels;
using SQLite;

namespace OwlReadingRoom.Services.Transactions;
public interface ITransactionService
{
    TableQuery<Transaction> TableQuery { get; }

    /// <summary>
    /// Retrieves transaction details for a given booking.
    /// </summary>
    /// <param name="bookingId">The ID of the booking.</param>
    /// <param name="hasParking">Indicates whether the booking includes parking.</param>
    /// <param name="hasLocker">Indicates whether the booking includes a locker.</param>
    /// <returns>A TransactionDetailViewModel containing transaction details, or null if the bookingId is not provided.</returns>
    TransactionDetailViewModel GetTransactionDetails(int? bookingId, bool hasParking, bool hasLocker);
}

