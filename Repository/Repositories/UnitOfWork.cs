using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Repository.Data;
using Repository.Entities;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class UnitOfWork : IDisposable, IUnitOfWork
    {
        private readonly InnoSphereDBContext _dbContext;
        private readonly Dictionary<Type, object> _repositories = new();
        private bool disposedValue;

        public UnitOfWork(InnoSphereDBContext dbContext)
        {
            _dbContext = dbContext;

        }

        public IGenericRepo<T> GetRepository<T>() where T : BaseEntity
        {
            if (!_repositories.ContainsKey(typeof(T)))
            {
                _repositories[typeof(T)] = new GenericRepo<T>(_dbContext);

            }
            return (IGenericRepo<T>)_repositories[typeof(T)];
        }

        public async Task<int> SaveChangesAsync()
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                var result = await _dbContext.SaveChangesAsync();
                await transaction.CommitAsync();
                return result;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _dbContext.Database.BeginTransactionAsync();
        }
    }
}
