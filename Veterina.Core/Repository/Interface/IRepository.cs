using System.Linq.Expressions;

namespace Veterina.Core.Repository.Interface
{
    public interface IRepository<TEntity> where TEntity : class
    {
        #region IRepository<T> Members

        Task<List<TEntity>> GetAllAsync(params Expression<Func<TEntity, object>>[] includeProperties);

        //Task<List<TEntity>> GetAllSelectAsync(params Expression<Func<TEntity, object>>[] includeProperties);

        /// Función que retorna una entidad, a partir de una consulta.
        Task<List<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includeProperties);

        //Task<List<TEntity>> FindAllSelectAsync(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includeProperties);

        /// Retorna la primera entidad encontrada bajo una condición especificada o null sino encontrara registros
        Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includeProperties);
        Task<TEntity?> FirstOrDefaultAsNoTrackingAsync(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includeProperties);
        /// Registra una entidad
        void Insert(TEntity entity);

        /// Registra varias entidades
        void Insert(IEnumerable<TEntity> entities);

        /// Actualiza una entidad
        void Update(TEntity entity);

        /// Actualiza varias entidades
        void Update(IEnumerable<TEntity> entities);

        /// Elimina una entidad
        void Delete(TEntity entity);

        /// Elimina por Id
        void Delete(object id);

        Task DeleteAsync(object id);

        /// Elimina un conjuto de entidades
        void Delete(IEnumerable<TEntity> entities);

        #endregion IRepository<T> Members
    }
}
