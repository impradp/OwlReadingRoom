using OwlReadingRoom.Models;
using SQLite;

namespace OwlReadingRoom.Services.Transactions;
public interface ITransactionService
{
    TableQuery<Transaction> TableQuery { get; }
}

