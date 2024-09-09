using OwlReadingRoom.Models;
using OwlReadingRoom.Services.Repository;
using SQLite;

namespace OwlReadingRoom.Services.Transactions;

public class TransactionService : ITransactionService
{
    private readonly IRepository<Transaction> _transactionRepository;

    public TransactionService(IRepository<Transaction> transactionRepository)
    {
        _transactionRepository = transactionRepository;
    }

    public TableQuery<Models.Transaction> TableQuery => _transactionRepository.Table;
}
