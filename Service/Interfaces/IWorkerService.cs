using Repository.Entities;
using Service.Models.WorkerModels;

namespace Service.Interfaces
{
    public interface IWorkerService
    {
        Task<Worker> CreateProfileAsync(string userId, WorkerEditModel request);
        Task<Worker> GetProfileAsync(string userId);
        Task<Worker> UpdateProfileAsync(string userId, WorkerEditModel request);
    }
}