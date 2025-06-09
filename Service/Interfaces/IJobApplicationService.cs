using Service.Models.JobApplicationModels;

namespace Service.Interfaces
{
    public interface IJobApplicationService
    {
        Task<JobApplicationModel> ApplyAsync(CreateJobApplicationModel model, int workerId);
        Task<bool> CancelApplicationAsync(int id, int workerId);
        Task<IEnumerable<JobApplicationModel>> GetByEmployerAsync(int employerId, int? jobPostingId = null);
        Task<JobApplicationModel> GetByIdAsync(int id);
        Task<IEnumerable<JobApplicationModel>> GetByWorkerAsync(int workerId);
        Task<bool> UpdateStatusAsync(int id, UpdateJobApplicationStatusModel model);
    }
}