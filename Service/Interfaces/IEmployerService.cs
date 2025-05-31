using Repository.Entities;
using Service.Models.EmployerModels;

namespace Service.Interfaces
{
    public interface IEmployerService
    {
        Task<Employer> CreateProfileAsync(string userId, EmployerEditModel request);
        Task<Employer> GetProfileAsync(string userId);
        Task<Employer> UpdateProfileAsync(string userId, EmployerEditModel request);
    }
}