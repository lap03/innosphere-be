using Service.Models.PaymentTypeModels;

namespace Service.Interfaces
{
    public interface IPaymentTypeService
    {
        Task<PaymentTypeModel> CreateAsync(CreatePaymentTypeModel dto);
        Task<bool> DeleteAsync(int id);
        Task<List<PaymentTypeModel>> GetAllActiveAsync();
        Task<List<PaymentTypeModel>> GetAllAsync();
        Task<PaymentTypeModel> GetByIdAsync(int id);
        Task<bool> HardDeleteAsync(int id);
        Task<bool> RestoreAsync(int id);
        Task<PaymentTypeModel> UpdateAsync(int id, UpdatePaymentTypeModel dto);
    }
}