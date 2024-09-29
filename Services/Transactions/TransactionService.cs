using Microsoft.Extensions.Configuration;
using OwlReadingRoom.Models;
using OwlReadingRoom.Services.Constants;
using OwlReadingRoom.Services.Repository;
using OwlReadingRoom.ViewModels;
using SQLite;

namespace OwlReadingRoom.Services.Transactions;

public class TransactionService : ITransactionService
{
    private readonly IRepository<Transaction> _transactionRepository;
    private readonly IRepository<TransactionLogs> _transactionLogsRepository;

    public TransactionService(IRepository<Transaction> transactionRepository, IRepository<TransactionLogs> transactionLogsRepository)
    {
        _transactionRepository = transactionRepository;
        _transactionLogsRepository = transactionLogsRepository;
    }

    public TableQuery<Models.Transaction> TableQuery => _transactionRepository.Table;
    public TransactionDetailViewModel GetTransactionDetails(int? bookingId)
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
                        LockerAmount = transaction.LockerAmount,
                        ParkingAmount = transaction.ParkingAmount,
                        LastPaymentDate = latestLog != null ? latestLog.PaymentDate : (DateTime?)null,
                    };
        return query.FirstOrDefault();
    }


    public void SaveTransaction(Transaction transaction)
    {
        _transactionRepository.SaveItem(transaction);
    }

    public void SaveTransactionLog(TransactionLogs log)
    {
        _transactionLogsRepository.SaveItem(log);
    }
}
