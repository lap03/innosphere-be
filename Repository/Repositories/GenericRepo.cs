using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.Entities;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class GenericRepo<TEntity> : IGenericRepo<TEntity> where TEntity : BaseEntity
    {
        private readonly InnoSphereDBContext _dbContext;

        public GenericRepo(InnoSphereDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        //GET
        public IQueryable<TEntity> GetAllQueryable()
        {
            return _dbContext.Set<TEntity>().AsQueryable();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate = null, params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = _dbContext.Set<TEntity>();

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            return await query.ToListAsync();
        }

        public async Task<(IEnumerable<TEntity> Items, int TotalCount)> GetAllByConditionAsync(
            Expression<Func<TEntity, bool>> predicate = null,
            int pageNumber = 1,
            int pageSize = int.MaxValue,
            params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = _dbContext.Set<TEntity>();

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            // Apply includes
            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            // Get total count for pagination info
            int totalCount = await query.CountAsync();

            // Apply pagination
            var pagedItems = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (pagedItems, totalCount);
        }

        public async Task<TEntity?> GetByIdAsync(int id, params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = _dbContext.Set<TEntity>();

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            var result = await query.FirstOrDefaultAsync(x => x.Id == id);
            return result;
        }

        public async Task<TEntity> GetSingleByConditionAsynce(Expression<Func<TEntity, bool>> predicate = null, params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = _dbContext.Set<TEntity>();

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            return await query.FirstOrDefaultAsync(predicate);
        }

        //ADD
        public async Task<TEntity> AddAsync(TEntity entity)
        {
            try
            {
                var result = await _dbContext.Set<TEntity>().AddAsync(entity);
                return result.Entity;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task AddRangeAsync(List<TEntity> entities)
        {
            try
            {
                await _dbContext.Set<TEntity>().AddRangeAsync(entities);
            }
            catch (Exception)
            {
                throw;
            }
        }

        //DELETE
        public async Task<bool> HardDelete(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                var entities = await _dbContext.Set<TEntity>().Where(predicate).ToListAsync();
                if (entities.Any())
                {
                    _dbContext.Set<TEntity>().RemoveRange(entities);
                    return true;
                }
                return false; // Không có gì để xóa
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while performing hard remove: {ex.Message}");
            }
        }

        public async Task<bool> HardDeleteRange(List<TEntity> entities)
        {
            try
            {
                if (entities.Any())
                {
                    _dbContext.Set<TEntity>().RemoveRange(entities);
                    return true;
                }
                return false; // Không có gì để xóa
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while performing hard remove range: {ex.Message}");
            }
        }

        public async Task<bool> SoftDelete(TEntity entity)
        {
            entity.IsDeleted = true;
            _dbContext.Set<TEntity>().Update(entity);
            return true;
        }

        public async Task<bool> SoftDeleteRange(List<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                entity.IsDeleted = true;
            }
            _dbContext.Set<TEntity>().UpdateRange(entities);
            //  await _dbContext.SaveChangesAsync();
            return true;
        }


        //UPDATE
        public async Task<bool> Update(TEntity entity)
        {
            _dbContext.Set<TEntity>().Update(entity);
            return true;
        }

        public async Task<bool> UpdateRange(List<TEntity> entities)
        {
            _dbContext.Set<TEntity>().UpdateRange(entities);
            return true;
        }

        public async Task CommitAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
