using Service.Models.WorkerRatingCriteriaModels;
using Service.Models.WorkerRatingModels;

namespace Service.Interfaces
{
    public interface IWorkerRatingService
    {
        Task CreateWorkerRatingAsync(CreateWorkerRatingModel model);
        Task<IEnumerable<WorkerRatingModel>> GetAllRatingsByWorkerAsync(int workerId);
        Task<IEnumerable<WorkerRatingCriteriaModel>> GetRatingDetailsAsync(int workerRatingId);
    }
}