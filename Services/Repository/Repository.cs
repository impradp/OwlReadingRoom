using OwlReadingRoom.Models;
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
        public Repository(IDatabaseConnectionService connectionService)
        {
            this.connectionService = connectionService;
        }

        public List<T> GetItems()
        {
            connectionService.Init<T>();
            List<T> items = connectionService.Connection.Table<T>().ToList();
            connectionService.CloseConnection();
            return items;
        }

        public T GetItems(Expression<Func<T, bool>> predicate)
        {
            connectionService.Init<T>();
            T item = connectionService.Connection.Table<T>().Where(predicate).FirstOrDefault();
            connectionService.CloseConnection();
            return item;
        }

        public List<T> Get<TValue>(Expression<Func<T, bool>> predicate = null, Expression<Func<T, TValue>> orderBy = null)
        {
            connectionService.Init<T>();
            var query = connectionService.Connection.Table<T>();

            if (predicate != null)
                query = query.Where(predicate);

            if (orderBy != null)
                query = query.OrderBy<TValue>(orderBy);

            List<T> items = query.ToList();
            connectionService.CloseConnection();
            return items;
        }

        public T GetItem(int id)
        {
            connectionService.Init<T>();
            T item = connectionService.Connection.Table<T>().Where(model => model.Id == id).FirstOrDefault();
            connectionService.CloseConnection();
            return item;
        }

        public int SaveItem(T item)
        {
            DateTime now = DateTime.UtcNow;
            int id;
            connectionService.Init<T>();
            if (item.Id != 0)
            {
                item.UpdatedAt = now;
                id = connectionService.Connection.Update(item);
            }
            else
            {
                item.CreatedAt = now;
                item.UpdatedAt = now;
                id = connectionService.Connection.Insert(item);
            }
            connectionService.CloseConnection();
            return id;
        }

        public int DeleteItem(T item)
        {
            connectionService.Init<T>();
            int id = connectionService.Connection.Delete(item);
            connectionService.CloseConnection();    
            return id;
        }

        public int InsertAll(IEnumerable<T> objects)
        {
            connectionService.Init<T>();
            int noOfRows = connectionService.Connection.InsertAll(objects);
            connectionService.CloseConnection();
            return noOfRows;
        }
    }
}
