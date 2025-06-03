using Repository.Entities;
using Service.Models.EmployerModels;

namespace Service.Interfaces
{
    public interface IEmployerService
    {
        Task<EmployerModel> CreateProfileAsync(string userId, EmployerEditModel request);
        Task<EmployerModel> GetProfileAsync(string userId);
        Task<EmployerModel> UpdateProfileAsync(string userId, EmployerEditModel request);
    }
}