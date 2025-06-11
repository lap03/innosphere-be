using Microsoft.AspNetCore.Identity;
using Repository.Entities;
using Service.Models.UserModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Service.Interfaces;

namespace Service.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;

        public UserService(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IMapper mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
        }

        // Lấy tất cả user kèm role
        public async Task<IEnumerable<UserWithRoleModel>> GetAllAsync()
        {
            var users = _userManager.Users.ToList();
            var result = new List<UserWithRoleModel>();

            foreach (var user in users)
            {
                var roles = (await _userManager.GetRolesAsync(user)).ToList();
                var userModel = _mapper.Map<UserWithRoleModel>(user);
                userModel.Roles = roles;
                result.Add(userModel);
            }
            return result;
        }

        public async Task<UserModel?> GetByIdAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return null;
            return _mapper.Map<UserModel>(user);
        }

        public async Task<bool> UpdateAsync(UpdateUserModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null) return false;

            user.FullName = model.FullName;
            user.PhoneNumber = model.PhoneNumber;

            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }

        // Ban (xóa mềm) user
        public async Task<bool> BanAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null || user.IsDeleted) return false;

            user.IsDeleted = true;
            user.DeletedAt = DateTime.UtcNow;
            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }

        // Unban user
        public async Task<bool> UnbanAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null || !user.IsDeleted) return false;

            user.IsDeleted = false;
            user.DeletedAt = null;
            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }
    }
}
