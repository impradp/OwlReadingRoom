
using OwlReadingRoom.Model;
using OwlReadingRoom.Services.Constants;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OwlReadingRoom.Services.Repository
{
    public class Repository<T> where T : BaseModel, new()
    {
        private SQLiteAsyncConnection Database;
        public Repository() { }

        async Task Init()
        {
            if (Database is not null)
                return;
            Database = new SQLiteAsyncConnection(DataBaseConstants.DatabasePath, DataBaseConstants.Flags);
            await Database.CreateTableAsync<T>();
        }

        public async Task<List<T>> GetItemsAsync()
        {
            await Init();
            return await Database.Table<T>().ToListAsync();
        }

        public async Task<T> GetItemsAsync(Expression<Func<T, bool>> predicate)
        {
            await Init();

            return await Database.Table<T>().Where(predicate).FirstOrDefaultAsync();
        }

        public async Task<List<T>> Get<TValue>(Expression<Func<T, bool>> predicate = null, Expression<Func<T, TValue>> orderBy = null)
        {
            await Init();
            var query = Database.Table<T>();

            if (predicate != null)
                query = query.Where(predicate);

            if (orderBy != null)
                query = query.OrderBy<TValue>(orderBy);

            return await query.ToListAsync();
        }

        public async Task<T> GetItemAsync(int id)
        {
            await Init();
            return await Database.Table<T>().Where(model => model.ID == id).FirstOrDefaultAsync();
        }

        public async Task<int> SaveItemAsync(T item)
        {
            await Init();
            if (item.ID != 0)
                return await Database.UpdateAsync(item);
            else
                return await Database.InsertAsync(item);
        }

        public async Task<int> DeleteItemAsync(T item)
        {
            await Init();
            return await Database.DeleteAsync(item);
        }
    }
}
