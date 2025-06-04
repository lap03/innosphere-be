using Repository.Entities;
using Service.Models.WorkerModels;

namespace Service.Interfaces
{
    public interface IWorkerService
    {
        Task<WorkerModel> CreateProfileAsync(string userId, WorkerEditModel request);
        Task<WorkerModel> GetProfileAsync(string userId);
        Task<WorkerModel> UpdateProfileAsync(string userId, WorkerEditModel model);
    }
}