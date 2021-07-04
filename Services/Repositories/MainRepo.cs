using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Services.Repositories
{
    public class MainRepo<TEntity> : IMainRepo<TEntity> where TEntity : class
    {
        private readonly NimaProjContext _context;
        private readonly DbSet<TEntity> _dbSet;

        public MainRepo(NimaProjContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }

        public virtual EntityEntry Add(TEntity entity)
        {
            return _dbSet.Add(entity);
        }

        public virtual async Task<EntityEntry> AddAsync(TEntity entity)
        {
            return await _dbSet.AddAsync(entity);
        }

        public virtual bool Update(TEntity entity)
        {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            return true;
        }

        public virtual bool Delete(TEntity entity)
        {
            try
            {
                if (_context.Entry(entity).State == EntityState.Detached)
                    _dbSet.Attach(entity);
                _dbSet.Remove(entity);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public virtual bool DeleteById(object id)
        {
            try
            {
                TEntity entity = _dbSet.Find(id);
                return entity != null && Delete(entity);
            }
            catch
            {
                return false;
            }
        }

        public virtual async Task<bool> DeleteByIdAsync(object id)
        {
            try
            {
                TEntity entity = await _dbSet.FindAsync(id);
                return entity != null && Delete(entity);
            }
            catch
            {
                return false;
            }
        }

        public virtual IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> where = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includes = null)
        {
            IQueryable<TEntity> query = _dbSet;
            if (where != null)
                query = query.Where(where);
            if (orderBy != null)
                query = orderBy(query);
            if (includes != null)
                foreach (string i in includes.Split(','))
                    query = query.Include(i);
            return query.ToList();
        }

        public virtual async Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> where = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includes = null)
        {
            IQueryable<TEntity> query = _dbSet;
            if (where != null)
                query = query.Where(where);
            if (orderBy != null)
                query = orderBy(query);
            if (includes != null)
                foreach (string i in includes.Split(','))
                    query = query.Include(i);
            return await query.ToListAsync();
        }

        public virtual bool Any(Expression<Func<TEntity, bool>> where = null)
        {
            if (where != null)
                return _dbSet.Any(where);
            return false;
        }

        public virtual async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> where = null)
        {
            if (where != null)
                return await _dbSet.AnyAsync(where);
            return false;
        }

        public virtual TEntity GetById(object id)
        {
            return _dbSet.Find(id);
        }

        public virtual async Task<TEntity> GetByIdAsync(object id)
        {
            return await _dbSet.FindAsync(id);
        }

        public virtual TEntity SingleOrDefault(Expression<Func<TEntity, bool>> where = null)
        {
            if (where != null)
                return _dbSet.FirstOrDefault(where);
            return null;
        }

        public virtual async Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> where = null)
        {
            if (where != null)
                return await _dbSet.FirstOrDefaultAsync(where);
            return null;
        }
    }
}
