using Application.Repositories;
using Domain.Entities.Common;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Infrastructure.Repositories;

public class EfRepositoryBase<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
{
    private readonly AppDbContext _dbContext;
    protected DbSet<TEntity> DbContext => _dbContext.Set<TEntity>(); 
    public EfRepositoryBase(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task CreateAsync(TEntity entity, CancellationToken cancellationToken)
    {
        await DbContext.AddAsync(entity, cancellationToken);
    }

    public void Delete(TEntity entity, CancellationToken cancellationToken)
    {
        DbContext.Remove(entity);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken)
    {
        TEntity entity = await GetByIdAsync(id, cancellationToken);
        Delete(entity,cancellationToken);
    }

    public async Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken, bool tracking = false)
    {
        if (!tracking)
            return await DbContext.AsNoTracking().ToListAsync(cancellationToken);
        
        return await DbContext.ToListAsync(cancellationToken);
    }

    public async Task<TEntity> GetByIdAsync(int id, CancellationToken cancellationToken, bool tracking = false)
    {
        if (!tracking)
            return await DbContext.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancellationToken) ?? throw new Exception("Not found");
        return await DbContext.FirstOrDefaultAsync(x => x.Id == id, cancellationToken) ?? throw new Exception("Not found");
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken)
    {
        _dbContext.Entry(entity).State = EntityState.Modified;
    }

    public async Task<List<TEntity>> GetWhereAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, bool tracking = false)
    {
        IQueryable<TEntity> query = DbContext;
        if (!tracking)
            query = query.AsNoTracking();
        query = query.Where(predicate);
        if (orderBy != null)
            query = orderBy(query);
        if (include != null)
            query = include(query);
        
        return await query.ToListAsync(cancellationToken);
    }
}

