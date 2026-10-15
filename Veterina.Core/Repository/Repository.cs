using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Veterina.Core.Repository.Interface;

namespace Veterina.Core.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly DbContext _dbcontext;
        private readonly DbSet<TEntity> _entities;

        public Repository(DbContext dbcontext)
        {
            this._dbcontext = dbcontext;
            this._entities = dbcontext.Set<TEntity>();
        }

        private IQueryable<TEntity> PerformInclusions(IEnumerable<Expression<Func<TEntity, object>>> includeProperties,
                                               IQueryable<TEntity> query)
        {
            return includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
        }

        #region IRepository<TEntity> Members

        public IQueryable<TEntity> AsQueryable()
        {
            return _entities.AsQueryable<TEntity>();
        }

        public async Task<List<TEntity>> GetAllAsync(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = AsQueryable().AsNoTracking();
            return await PerformInclusions(includeProperties, query).ToListAsync();
        }


        public async Task<List<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = AsQueryable().AsNoTracking();
            query = PerformInclusions(includeProperties, query);
            return await query.Where(where).ToListAsync();
        }

        public async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = AsQueryable();
            query = PerformInclusions(includeProperties, query);
            return await query.FirstOrDefaultAsync(where);
        }

        public async Task<TEntity?> FirstOrDefaultAsNoTrackingAsync(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = AsQueryable().AsNoTracking();
            query = PerformInclusions(includeProperties, query);
            return await query.FirstOrDefaultAsync(where);
        }

        public void Insert(TEntity entity)
        {
            _entities.Add(entity);
        }

        public void Insert(IEnumerable<TEntity> entities)
        {
            foreach (var e in entities)
            {
                _dbcontext.Entry(e).State = EntityState.Added;
            }
        }

        public void Update(TEntity entity)
        {
            _entities.Attach(entity);
            _dbcontext.Entry(entity).State = EntityState.Modified;
        }

        public void Update(IEnumerable<TEntity> entities)
        {
            foreach (var e in entities)
            {
                _dbcontext.Entry(e).State = EntityState.Modified;
            }
        }

        public void Delete(TEntity entity)
        {
            if (_dbcontext.Entry(entity).State == EntityState.Detached)
            {
                _entities.Attach(entity);
            }
            _entities.Remove(entity);
        }

        public void Delete(IEnumerable<TEntity> entities)
        {
            foreach (var e in entities)
            {
                _dbcontext.Entry(e).State = EntityState.Deleted;
            }
        }

        public void Delete(object id)
        {
            TEntity entityToDelete = _entities.Find(id);
            _entities.Remove(entityToDelete);
        }

        public async Task DeleteAsync(object id)
        {
            TEntity? entityToDelete = await _entities.FindAsync(id);
            if (entityToDelete != null)
                _entities.Remove(entityToDelete);
        }

        #endregion IRepository<TEntity> Members

    }
}
