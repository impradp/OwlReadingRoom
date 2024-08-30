using OwlReadingRoom.Models;
using SQLite;
using System.Linq.Expressions;

namespace OwlReadingRoom.Services.Repository
{
    public interface IRepository<T> where T : BaseModel, new()
    {
        TableQuery<T> Table { get; }
        int DeleteItem(T item);
        List<T> Get<TValue>(Expression<Func<T, bool>> predicate = null, Expression<Func<T, TValue>> orderBy = null);
        T GetItem(int id);
        List<T> GetItems();
        T GetItems(Expression<Func<T, bool>> predicate);
        int SaveItem(T item);
        int InsertAll(IEnumerable<T> objects);
    }
}
