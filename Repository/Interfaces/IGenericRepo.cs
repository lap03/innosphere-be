using Repository.Entities;
using System.Linq.Expressions;

namespace Repository.Interfaces
{
    public interface IGenericRepo<TEntity> where TEntity : BaseEntity
    {
        Task<TEntity> AddAsync(TEntity entity);
        Task AddRangeAsync(List<TEntity> entities);
        Task CommitAsync();
        Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate = null, params Expression<Func<TEntity, object>>[] includes);
        Task<(IEnumerable<TEntity> Items, int TotalCount)> GetAllByConditionAsync(Expression<Func<TEntity, bool>> predicate = null, int pageNumber = 1, int pageSize = int.MaxValue, params Expression<Func<TEntity, object>>[] includes);
        IQueryable<TEntity> GetAllQueryable();
        Task<TEntity?> GetByIdAsync(int id, params Expression<Func<TEntity, object>>[] includes);
        Task<TEntity> GetSingleByConditionAsynce(Expression<Func<TEntity, bool>> predicate = null, params Expression<Func<TEntity, object>>[] includes);
        Task<bool> HardDelete(Expression<Func<TEntity, bool>> predicate);
        Task<bool> HardDeleteRange(List<TEntity> entities);
        Task<bool> SoftDelete(TEntity entity);
        Task<bool> SoftDeleteRange(List<TEntity> entities);
        Task<bool> Update(TEntity entity);
        Task<bool> UpdateRange(List<TEntity> entities);
    }
}