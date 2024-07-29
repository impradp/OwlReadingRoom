using OwlReadingRoom.Model;
using OwlReadingRoom.Services.Constants;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwlReadingRoom.Services.Database
{
    public class DatabaseConnectionService : IDatabaseConnectionService
    {
        private SQLiteAsyncConnection connection;
        public SQLiteAsyncConnection Connection { get => connection; }

        public async Task Init<T>() where T : BaseModel, new()
        {
            if (connection is not null)
                return;
            connection = new SQLiteAsyncConnection(DataBaseConstants.DatabasePath, DataBaseConstants.Flags);
            await connection.CreateTableAsync<T>();
        }
    }
}
