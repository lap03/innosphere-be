using Service.Models.PaymentModels;

namespace Service.Interfaces
{
    public interface IPaymentService
    {
        Task<PaymentModel> CreateAsync(CreatePaymentModel dto);
        Task<bool> DeleteAsync(int id);
        Task<List<PaymentModel>> GetAllAsync();
        Task<PaymentModel> GetByIdAsync(int id);
        Task<PaymentModel> UpdateAsync(int id, UpdatePaymentModel dto);
    }
}