using Service.Models.ResumeModels;

namespace Service.Interfaces
{
    public interface IResumeService
    {
        Task<ResumeModel> AddResumeAsync(CreateResumeModel model);
        Task DeleteResumeAsync(int id);
        Task<ResumeModel> GetResumeAsync(int id);
        Task<IEnumerable<ResumeModel>> GetResumesByWorkerAsync(int workerId);
        Task RestoreResumeAsync(int id);
        Task<ResumeModel> UpdateResumeAsync(int id, CreateResumeModel model);
    }
}