using FormsBackendCommon.Interface;
using FormsBackendCommon.Model;
using System.Linq.Expressions;

namespace FormBackendTest.Mock;

public enum RepositoryOperationType
{
    Insert, Update, Delete, Get, GetById, GetFiltered, SaveChanges
};

public record RepositoryOperation(RepositoryOperationType OperationType, List<object> Arguments);

public class MockGenericRepository<T> : IGenericRepository<T> where T : BaseEntity
{
    public List<RepositoryOperation> Operations { get; } = [];

    public List<T> Models { get; set; } = [];

    public async Task<int> InsertAsync(T user)
    {
        Operations.Add(new RepositoryOperation(RepositoryOperationType.Insert, [user]));
        return await Task.FromResult(user.Id);
    }

    public async Task UpdateAsync(T user)
    {
        Operations.Add(new RepositoryOperation(RepositoryOperationType.Update, [user]));
        await Task.CompletedTask;
    }

    public async Task<List<T>> GetAsync(int? skip, int? take, params Expression<Func<T, object>>[] includes)
    {
        Operations.Add(new RepositoryOperation(RepositoryOperationType.Get, [skip, take, includes]));
        return await Task.FromResult(Models);
    }

    public async Task<T?> GetByIdAsync(int id, params Expression<Func<T, object>>[] includes)
    {
        Operations.Add(new RepositoryOperation(RepositoryOperationType.GetById, [id, includes]));
        return await Task.FromResult(Models.Find(user => user.Id == id));
    }

    public async Task<List<T>> GetFilteredAsync(Expression<Func<T, bool>>[] filters, int? skip = null, int? take = null, params Expression<Func<T, object>>[] includes)
    {
        Operations.Add(new RepositoryOperation(RepositoryOperationType.GetFiltered, [filters, skip, take, includes]));

        var query = Models;
        foreach (var filter in filters)
            query = query.Where(filter.Compile()).ToList();

        return await Task.FromResult(query.ToList());
    }

    public async Task DeleteAsync(T user)
    {
        Operations.Add(new RepositoryOperation(RepositoryOperationType.Delete, [user]));
        await Task.CompletedTask;
    }

    public async Task SaveChangesAsync()
    {
        Operations.Add(new RepositoryOperation(RepositoryOperationType.SaveChanges, []));
        await Task.CompletedTask;
    }
}
