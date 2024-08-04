using OwlReadingRoom.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwlReadingRoom.Services.Database
{
    public interface IDatabaseConnectionService
    {
        SQLiteAsyncConnection Connection { get; }
        Task Init<T>() where T : BaseModel, new();
    }
}
