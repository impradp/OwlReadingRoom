
using OwlReadingRoom.Model;
using OwlReadingRoom.Services.Constants;
using OwlReadingRoom.Services.Database;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OwlReadingRoom.Services.Repository
{
    public class Repository<T> : IRepository<T> where T : BaseModel, new()
    {
        private readonly IDatabaseConnectionService connectionService;
        public Repository(IDatabaseConnectionService connectionService) {
            this.connectionService = connectionService;
        }

        public async Task<List<T>> GetItemsAsync()
        {
            await connectionService.Init<T>();
            return await connectionService.Connection.Table<T>().ToListAsync();
        }

        public async Task<T> GetItemsAsync(Expression<Func<T, bool>> predicate)
        {
            await connectionService.Init<T>();

            return await connectionService.Connection.Table<T>().Where(predicate).FirstOrDefaultAsync();
        }

        public async Task<List<T>> Get<TValue>(Expression<Func<T, bool>> predicate = null, Expression<Func<T, TValue>> orderBy = null)
        {
            await connectionService.Init<T>();
            var query = connectionService.Connection.Table<T>();

            if (predicate != null)
                query = query.Where(predicate);

            if (orderBy != null)
                query = query.OrderBy<TValue>(orderBy);

            return await query.ToListAsync();
        }

        public async Task<T> GetItemAsync(int id)
        {
            await connectionService.Init<T>();
            return await connectionService.Connection.Table<T>().Where(model => model.Id == id).FirstOrDefaultAsync();
        }

        public async Task<int> SaveItemAsync(T item)
        {
            DateTime now = DateTime.UtcNow;
            await connectionService.Init<T>();
            if (item.Id != 0)
            {
                item.UpdatedAt = now;
                return await connectionService.Connection.UpdateAsync(item);
            }
            else
            {
                item.CreatedAt = now;
                item.UpdatedAt = now;
                return await connectionService.Connection.InsertAsync(item);
            }             
        }

        public async Task<int> DeleteItemAsync(T item)
        {
            await connectionService.Init<T>();
            return await connectionService.Connection.DeleteAsync(item);
        }
    }
}
