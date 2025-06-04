using Service.Models.JobTagModels;

namespace Service.Interfaces
{
    public interface IJobTagService
    {
        Task<JobTagModel> CreateAsync(CreateJobTagModel dto);
        Task<bool> DeleteAsync(int id);
        Task<List<JobTagModel>> GetAllActiveAsync();
        Task<List<JobTagModel>> GetAllAsync();
        Task<JobTagModel> GetByIdAsync(int id);
        Task<bool> HardDeleteAsync(int id);
        Task<bool> RestoreAsync(int id);
        Task<JobTagModel> UpdateAsync(int id, UpdateJobTagModel dto);
    }
}