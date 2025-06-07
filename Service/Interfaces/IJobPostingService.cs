using Service.Models.JobPostings;
using Service.Models.PagedResultModels;

namespace Service.Interfaces
{
    public interface IJobPostingService
    {
        Task<JobPostingModel> CreateJobPostingAsync(CreateJobPostingModel model, List<int> tagIds);
        Task<JobPostingModel?> GetJobPostingByIdAsync(int id);
        Task<PagedResultModel<JobPostingModel>> GetJobPostingsAsync(JobPostingFilterModel filter);
        Task<IEnumerable<JobPostingModel>> GetJobPostingsByEmployerAsync(int employerId, string? status = null);
        Task<bool> UpdateJobPostingStatusAsync(int jobPostingId, string status);
    }
}