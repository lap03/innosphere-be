using Service.Models.JobPostings;

namespace Service.Interfaces
{
    public interface IJobPostingService
    {
        Task<JobPostingModel> CreateJobPostingAsync(CreateJobPostingModel model, List<int> tagIds);
        Task<JobPostingModel?> GetJobPostingByIdAsync(int id);
        Task<IEnumerable<JobPostingModel>> GetJobPostingsByEmployerAsync(int employerId);
        Task<bool> UpdateJobPostingStatusAsync(int jobPostingId, string status);
    }
}