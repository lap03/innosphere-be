using Service.Models.SocialLinkModels;

namespace Service.Interfaces
{
    public interface ISocialLinkService
    {
        Task<SocialLinkModel> CreateAsync(CreateSocialLinkModel dto, string userId);
        Task<bool> DeleteAsync(int id);
        Task<List<SocialLinkModel>> GetAllActiveAsync();
        Task<SocialLinkModel> GetByIdAsync(int id);
        Task<List<SocialLinkModel>> GetByUserIdAsync(string userId);
        Task<bool> HardDeleteAsync(int id);
        Task<bool> RestoreAsync(int id);
        Task<SocialLinkModel> UpdateAsync(int id, UpdateSocialLinkModel dto);
    }
}