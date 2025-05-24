using Repository.Entities;

namespace Repository.Interfaces
{
    public interface IUnitOfWork
    {
        void Dispose();
        IGenericRepo<T> GetRepository<T>() where T : BaseEntity;
        Task<int> SaveChangesAsync();
    }
}