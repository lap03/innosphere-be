using Service.Models.AdvertisementModels;

public interface IAdvertisementService
{
    Task<AdvertisementModel> CreateAsync(CreateAdvertisementModel dto);
    Task<List<AdvertisementModel>> GetAllActiveByEmployerAsync(int employerId);
    Task<List<AdvertisementModel>> GetAllByEmployerAsync(int employerId);
    Task<AdvertisementModel> GetByIdAsync(int id, int employerId);
    Task<bool> HardDeleteAsync(int id, int employerId);
    Task<bool> IncrementImpressionAsync(int id);
    Task<bool> RestoreAsync(int id, int employerId);
    Task<bool> SoftDeleteAsync(int id, int employerId);
    Task<AdvertisementModel> UpdateAsync(int id, int employerId, UpdateAdvertisementModel dto);
}