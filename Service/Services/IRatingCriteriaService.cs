using Service.Models.RatingCriteriaModels;

namespace Service.Services
{
    public interface IRatingCriteriaService
    {
        Task<RatingCriteriaModel> AddAsync(CreateRatingCriteriaModel model);
        Task DeleteAsync(int id);
        Task<IEnumerable<RatingCriteriaModel>> GetAllAsync();
        Task<RatingCriteriaModel> GetByIdAsync(int id);
        Task<IEnumerable<RatingCriteriaModel>> GetByTypeAsync(string criteriaType);
        Task RestoreAsync(int id);
        Task<RatingCriteriaModel> UpdateAsync(int id, CreateRatingCriteriaModel model);
    }
}