using Domain.Entities.Common;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Application.Repositories;

public interface IRepository<TEntity> where TEntity : BaseEntity
{
    Task<TEntity> GetByIdAsync(int id, CancellationToken cancellationToken, bool tracking = false);
    Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken, bool tracking = false);
    Task<List<TEntity>> GetWhereAsync(
        Expression<Func<TEntity, bool>> predicate, 
        CancellationToken cancellationToken,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        bool tracking = false
    );


    Task CreateAsync(TEntity entity, CancellationToken cancellationToken);

    Task UpdateAsync(TEntity entity, CancellationToken cancellationToken);

    void Delete(TEntity entity, CancellationToken cancellationToken);
    Task DeleteAsync(int id, CancellationToken cancellationToken);

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);

}

