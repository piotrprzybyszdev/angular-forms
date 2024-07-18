using FormsBackendCommon.Interface;
using FormsBackendCommon.Model;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FormsBackendInfrastructure;

public class GenericRepository<T>(ApplicationDbContext context) : IGenericRepository<T> where T : BaseEntity
{
    private readonly DbSet<T> set = context.Set<T>();

    public async Task<int> InsertAsync(T entity)
    {
        await set.AddAsync(entity);
        return entity.Id;
    }

    public async Task UpdateAsync(T entity)
    {
        set.Attach(entity);
        context.Entry(entity).State = EntityState.Modified;
        await Task.CompletedTask;
    }

    public async Task<List<T>> GetAsync(int? skip, int? take, params Expression<Func<T, object>>[] includes)
    {
        IQueryable<T> query = set;

        foreach (var include in includes)
            query = query.Include(include);

        if (skip != null)
            query = query.Skip(skip.Value);

        if (take != null)
            query = query.Take(take.Value);

        return await query.ToListAsync();
    }

    public async Task<T?> GetByIdAsync(int id, params Expression<Func<T, object>>[] includes)
    {
        IQueryable<T> query = set;

        query = query.Where(entity => entity.Id == id);

        foreach (var include in includes)
            query = query.Include(include);

        return await query.SingleOrDefaultAsync();
    }

    public async Task<List<T>> GetFilteredAsync(Expression<Func<T, bool>>[] filters, int? skip, int? take, params Expression<Func<T, object>>[] includes)
    {
        IQueryable<T> query = set;

        foreach (var filter in filters)
            query = query.Where(filter);

        foreach (var include in includes)
            query = query.Include(include);

        if (skip != null)
            query = query.Skip(skip.Value);

        if (take != null)
            query = query.Take(take.Value);

        return await query.ToListAsync();
    }

    public async Task DeleteAsync(T entity)
    {
        if (context.Entry(entity).State == EntityState.Detached)
            set.Attach(entity);
        set.Remove(entity);
        await Task.CompletedTask;
    }

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }
}
