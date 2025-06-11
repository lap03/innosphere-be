using Service.Models.BusinessTypeModels;

namespace Service.Interfaces
{
    public interface IBusinessTypeService
    {
        Task<BusinessTypeModel> CreateAsync(CreateBusinessTypeModel model);
        Task<List<BusinessTypeModel>> GetAllAsync();
        Task<BusinessTypeModel> GetByIdAsync(int id);
        Task<bool> SoftDeleteAsync(int id);
        Task<BusinessTypeModel> UpdateAsync(int id, UpdateBusinessTypeModel model);
    }
}