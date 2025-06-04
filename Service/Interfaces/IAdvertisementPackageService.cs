using Service.Models.AdvertisementPackageModels;

namespace Service.Interfaces
{
    public interface IAdvertisementPackageService
    {
        Task<AdvertisementPackageModel> CreateAsync(CreateAdvertisementPackageModel dto);
        Task<bool> DeleteAsync(int id);
        Task<List<AdvertisementPackageModel>> GetAllActiveAsync();
        Task<List<AdvertisementPackageModel>> GetAllAsync();
        Task<AdvertisementPackageModel> GetByIdAsync(int id);
        Task<bool> HardDeleteAsync(int id);
        Task<bool> RestoreAsync(int id);
        Task<AdvertisementPackageModel> UpdateAsync(int id, UpdateAdvertisementPackageModel dto);
    }
}