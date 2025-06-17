using Service.Models.EmployerRatingCriteriaModels;
using Service.Models.EmployerRatingModels;

namespace Service.Interfaces
{
    public interface IEmployerRatingService
    {
        Task CreateEmployerRatingAsync(CreateEmployerRatingModel model);
        Task<IEnumerable<EmployerRatingModel>> GetAllRatingsByEmployerAsync(int employerId);
        Task<IEnumerable<EmployerRatingCriteriaModel>> GetRatingDetailsAsync(int employerRatingId);
    }
}