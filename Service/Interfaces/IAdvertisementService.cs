using Service.Models.AdvertisementModels;

public interface IAdvertisementService
{
    Task<AdvertisementModel> CreateAsync(CreateAdvertisementModel dto);
    Task<List<AdvertisementModel>> GetAllActiveAsync();
    Task<List<AdvertisementModel>> GetAllAsync();
    Task<AdvertisementModel> GetByIdAsync(int id);
    Task<bool> HardDeleteAsync(int id);
    Task<bool> IncrementImpressionAsync(int id);
    Task<bool> RestoreAsync(int id);
    Task<bool> SoftDeleteAsync(int id);
    Task<AdvertisementModel> UpdateAsync(int id, UpdateAdvertisementModel dto);
}