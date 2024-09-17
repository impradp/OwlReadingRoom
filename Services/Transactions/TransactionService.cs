using Microsoft.Extensions.Configuration;
using OwlReadingRoom.Models;
using OwlReadingRoom.Services.Repository;
using OwlReadingRoom.ViewModels;
using SQLite;

namespace OwlReadingRoom.Services.Transactions;

public class TransactionService : ITransactionService
{
    private readonly IRepository<Transaction> _transactionRepository;
    private readonly IRepository<TransactionLogs> _transactionLogsRepository;
    private readonly IConfiguration _configuration;

    public TransactionService(IRepository<Transaction> transactionRepository, IConfiguration configuration, IRepository<TransactionLogs> transactionLogsRepository)
    {
        _transactionRepository = transactionRepository;
        _configuration = configuration;
        _transactionLogsRepository = transactionLogsRepository;
    }

    public TableQuery<Models.Transaction> TableQuery => _transactionRepository.Table;
    public TransactionDetailViewModel GetTransactionDetails(int? bookingId, bool hasParking, bool hasLocker)
    {
        if (!bookingId.HasValue)
        {
            return null;
        }

        var query = from transaction in _transactionRepository.Table
                    join txnLogs in _transactionLogsRepository.Table on transaction.Id equals txnLogs.TransactionId into txnLogGroup
                    from latestLog in txnLogGroup.OrderByDescending(log => log.PaymentDate).Take(1).DefaultIfEmpty()
                    where transaction.BookingInformationId == bookingId
                    select new TransactionDetailViewModel
                    {
                        TransactionId = transaction.Id,
                        DueAmount = transaction.DueAmount,
                        TotalAmount = transaction.TotalAmount,
                        PaidAmount = transaction.PaidAmount,
                        LockerAmount = hasLocker ? GetFacilityPrice("Locker") : 0,
                        ParkingAmount = hasParking ? GetFacilityPrice("Parking") : 0,
                        LastPaymentDate = latestLog != null ? latestLog.PaymentDate : (DateTime?)null
                    };
        return query.FirstOrDefault();
    }

    /// <summary>
    /// Retrieves the price for a given facility.
    /// </summary>
    /// <param name="facility">The name of the facility.</param>
    /// <returns>The price of the facility as a double, or 0 if the price cannot be parsed.</returns>
    private double GetFacilityPrice(string facility)
    {
        var facilityPrices = _configuration.GetRequiredSection("FacilityPrices");
        return double.TryParse(facilityPrices.GetSection(facility).Value, out double price) ? price : 0;
    }
}
