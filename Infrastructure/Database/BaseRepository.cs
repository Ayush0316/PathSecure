using Domain.Entities;
using Microsoft.EntityFrameworkCore;
namespace Infrastructure.Database;

/// <summary>
/// Represents the base repository, providing common functionality for all derived repositories.
/// </summary>
public abstract class BaseRepository<T> : IRepository<T> where T : class
{
    protected BaseDbContext _dc;

    protected BaseRepository(BaseDbContext dc)
    {
        _dc = dc;
    }

    public IQueryable<T> Query
    {
        get
        {
            var q = _dc.Set<T>().AsQueryable();
            if (typeof(BaseEntity).IsAssignableFrom(typeof(T)))
            {
                q = q.Where(e => EF.Property<bool>(e, "IsDeleted") == false);
            }
            return q;
        }
    }

    public virtual async Task Create(T e, bool saveChanges = true)
    {
        await _dc.Set<T>().AddAsync(e);
        if (saveChanges)
            await _dc.SaveChangesAsync();
    }

    public virtual async Task Update(T e, bool saveChanges = true)
    {
        _dc.Set<T>().Update(e);
        if (saveChanges)
            await _dc.SaveChangesAsync();
    }

    public virtual async Task<T?> FindByPrimaryKey(params object[] pkVals)
    {
        ArgumentNullException.ThrowIfNull(pkVals, nameof(pkVals));
        return await _dc.Set<T>().FindAsync(pkVals);
    }

    public IAsyncEnumerable<T> FindAll() =>
        Query.AsAsyncEnumerable();

    public virtual async Task Delete(T e, bool trySoft = true, bool saveChanges = true)
    {
        if (trySoft && e is BaseEntity be)
        {
            be.IsDeleted = true;
            _dc.Set<T>().Update(e);
        }
        else
            _dc.Set<T>().Remove(e);

        if (saveChanges)
            await _dc.SaveChangesAsync();
    }

    public async Task SaveChanges()
    {
        await _dc.SaveChangesAsync();
    }
}

