using OwlReadingRoom.Models;
using SQLite;

namespace OwlReadingRoom.Services.Database
{
    public interface IDatabaseConnectionService : IDisposable
    {
        SQLiteConnection Connection { get; }
        void Init<T>() where T : BaseModel, new();
        void OpenConnection();
        void BeginTransaction();
        void CommitTransaction();
        void RollBack();
        SQLiteConnection OpenNewConnection();
    }
}
