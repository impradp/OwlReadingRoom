using OwlReadingRoom.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OwlReadingRoom.Services.Repository
{
    public interface IRepository<T> where T : BaseModel, new()
    {
        Task<int> DeleteItemAsync(T item);
        Task<List<T>> Get<TValue>(Expression<Func<T, bool>> predicate = null, Expression<Func<T, TValue>> orderBy = null);
        Task<T> GetItemAsync(int id);
        Task<List<T>> GetItemsAsync();
        Task<T> GetItemsAsync(Expression<Func<T, bool>> predicate);
        Task<int> SaveItemAsync(T item);
    }
}
