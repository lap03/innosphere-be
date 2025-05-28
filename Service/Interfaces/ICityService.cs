using Service.Models.CityModels;

namespace Service.Interfaces
{
    public interface ICityService
    {
        Task<CityModel> CreateAsync(CreateCityModel dto);
        Task<bool> DeleteAsync(int id);
        Task<List<CityModel>> GetAllAsync();
        Task<CityModel> GetByIdAsync(int id);
        Task<CityModel> UpdateAsync(int id, UpdateCityModel dto);
    }
}