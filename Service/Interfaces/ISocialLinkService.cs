using Service.Models.SocialLinkModels;

namespace Service.Interfaces
{
    public interface ISocialLinkService
    {
        Task<SocialLinkModel> CreateAsync(CreateSocialLinkModel dto);
        Task<bool> DeleteAsync(int id);
        Task<List<SocialLinkModel>> GetAllActiveAsync();
        Task<List<SocialLinkModel>> GetAllAsync();
        Task<SocialLinkModel> GetByIdAsync(int id);
        Task<bool> HardDeleteAsync(int id);
        Task<bool> RestoreAsync(int id);
        Task<SocialLinkModel> UpdateAsync(int id, UpdateSocialLinkModel dto);
        Task<List<SocialLinkModel>> GetByUserIdAsync(string userId);
    }
}