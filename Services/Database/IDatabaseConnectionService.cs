using OwlReadingRoom.Models;
using SQLite;

namespace OwlReadingRoom.Services.Database
{
    public interface IDatabaseConnectionService
    {
        SQLiteConnection Connection { get; }
        void Init<T>() where T : BaseModel, new();

        void CloseConnection();
    }
}
