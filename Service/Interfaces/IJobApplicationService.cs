using Service.Models.JobApplicationModels;

namespace Service.Interfaces
{
    public interface IJobApplicationService
    {
        Task<JobApplicationModel> ApplyAsync(CreateJobApplicationModel model, string userId);
        Task<bool> CancelApplicationAsync(int id, string userId);
        Task<IEnumerable<JobApplicationModel>> GetByEmployerAsync(string userId, int? jobPostingId = null);
        Task<JobApplicationModel> GetByIdAsync(int id);
        Task<IEnumerable<JobApplicationModel>> GetByWorkerAsync(string userId);
        Task<bool> UpdateStatusAsync(int id, UpdateJobApplicationStatusModel model);
    }
}