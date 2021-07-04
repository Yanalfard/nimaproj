using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;


namespace Services.Repositories
{
    public interface IMainRepo<TEntity> where TEntity : class
    {
        EntityEntry Add(TEntity entity);

        Task<EntityEntry> AddAsync(TEntity entity);

        bool Update(TEntity entity);

        bool Delete(TEntity entity);

        bool DeleteById(object id);

        Task<bool> DeleteByIdAsync(object id);

        TEntity GetById(object id);

        IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> where = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includes = null);

        Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> where = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includes = null);

        bool Any(Expression<Func<TEntity, bool>> where = null);

        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> where = null);

        Task<TEntity> GetByIdAsync(object id);

        TEntity SingleOrDefault(Expression<Func<TEntity, bool>> where = null);

        Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> where = null);


    }
}
