using OwlReadingRoom.Models;
using OwlReadingRoom.Services.Database;
using System.Linq.Expressions;


namespace OwlReadingRoom.Services.Repository
{
    public class Repository<T> : IRepository<T> where T : BaseModel, new()
    {
        private readonly IDatabaseConnectionService connectionService;
        private readonly IUserService _userService;
        public Repository(IDatabaseConnectionService connectionService, IUserService userService)
        {
            this.connectionService = connectionService;
            _userService = userService;
        }

        public List<T> GetItems()
        {
            connectionService.Init<T>();
            return connectionService.Connection.Table<T>().ToList();

        }

        public T GetItems(Expression<Func<T, bool>> predicate)
        {
            connectionService.Init<T>();
            return connectionService.Connection.Table<T>().Where(predicate).FirstOrDefault();
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
            return items;
        }

        public T GetItem(int id)
        {
            connectionService.Init<T>();
            return connectionService.Connection.Table<T>().Where(model => model.Id == id).FirstOrDefault();
        }

        public int SaveItem(T item)
        {
            DateTime now = DateTime.UtcNow;
            connectionService.Init<T>();
            if (item.Id != 0)
            {
                item.UpdatedAt = now;
                item.UpdatedBy = _userService.CurrentUser.Name;
                return connectionService.Connection.Update(item);
            }
            else
            {
                item.CreatedAt = now;
                item.UpdatedAt = now;
                item.CreatedBy = _userService.CurrentUser.Name;
                item.UpdatedBy = _userService.CurrentUser.Name;
                return connectionService.Connection.Insert(item);
            }
        }

        public int DeleteItem(T item)
        {
            connectionService.Init<T>();
            return connectionService.Connection.Delete(item);
        }

        public int InsertAll(IEnumerable<T> objects)
        {
            //TODO: insert the dates and audits
            connectionService.Init<T>();
            return connectionService.Connection.InsertAll(objects);
        }
    }
}
