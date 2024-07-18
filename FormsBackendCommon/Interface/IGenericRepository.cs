using FormsBackendCommon.Model;
using System.Linq.Expressions;

namespace FormsBackendCommon.Interface;

public interface IGenericRepository<T> where T : BaseEntity
{
    Task DeleteAsync(T entity);
    Task<List<T>> GetAsync(int? skip = null, int? take = null, params Expression<Func<T, object>>[] includes);
    Task<T?> GetByIdAsync(int id, params Expression<Func<T, object>>[] includes);
    Task<List<T>> GetFilteredAsync(Expression<Func<T, bool>>[] filters, int? skip = null, int? take = null, params Expression<Func<T, object>>[] includes);
    Task<int> InsertAsync(T entity);
    Task SaveChangesAsync();
    Task UpdateAsync(T entity);
}
