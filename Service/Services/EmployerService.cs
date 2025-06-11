using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Repository.Entities;
using Repository.Interfaces;
using Service.Interfaces;
using Service.Models.EmployerModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class EmployerService : IEmployerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public EmployerService(IUnitOfWork unitOfWork, IMapper mapper, UserManager<User> userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<EmployerModel> GetProfileAsync(string userId)
        {
            var employer = await _unitOfWork.GetRepository<Employer>()
                .GetSingleByConditionAsynce(e => e.UserId == userId, e => e.User);
            if (employer == null)
                throw new KeyNotFoundException("Employer profile not found.");

            return _mapper.Map<EmployerModel>(employer);
        }

        public async Task<EmployerModel> CreateProfileAsync(string userId, EmployerEditModel request)
        {
            var repo = _unitOfWork.GetRepository<Employer>();
            if (await repo.GetSingleByConditionAsynce(e => e.UserId == userId) != null)
                throw new InvalidOperationException("Employer profile already exists.");

            int businessTypeId;
            if (request.BusinessTypeId.HasValue)
            {
                var businessType = await _unitOfWork.GetRepository<BusinessType>().GetByIdAsync(request.BusinessTypeId.Value);
                if (businessType == null || businessType.IsDeleted)
                    throw new ArgumentException("Business type not found or not available.");
                businessTypeId = businessType.Id;
            }
            else if (!string.IsNullOrWhiteSpace(request.NewBusinessTypeName))
            {
                var newType = new BusinessType
                {
                    Name = request.NewBusinessTypeName.Trim(),
                    Description = request.NewBusinessTypeDescription,
                    IsDeleted = true,
                    CreatedAt = DateTime.UtcNow
                };
                await _unitOfWork.GetRepository<BusinessType>().AddAsync(newType);
                await _unitOfWork.SaveChangesAsync();
                businessTypeId = newType.Id;
            }
            else
            {
                throw new ArgumentException("Business type is required.");
            }

            var employer = _mapper.Map<Employer>(request);
            employer.UserId = userId;
            employer.BusinessTypeId = businessTypeId;
            employer.CreatedAt = DateTime.UtcNow;

            await repo.AddAsync(employer);
            await _unitOfWork.SaveChangesAsync();

            // Xử lý SocialLinks (nếu có)
            if (request.SocialLinks != null && request.SocialLinks.Count > 0)
            {
                var socialRepo = _unitOfWork.GetRepository<SocialLink>();
                foreach (var link in request.SocialLinks)
                {
                    var entity = _mapper.Map<SocialLink>(link);
                    entity.UserId = userId;
                    await socialRepo.AddAsync(entity);
                }
                await _unitOfWork.SaveChangesAsync();
            }

            return await GetProfileAsync(userId);
        }

        public async Task<EmployerModel> UpdateProfileAsync(string userId, EmployerEditModel request)
        {
            var repo = _unitOfWork.GetRepository<Employer>();
            var employer = await repo.GetSingleByConditionAsynce(e => e.UserId == userId, e => e.User);
            if (employer == null)
                throw new KeyNotFoundException("Employer profile not found.");

            int businessTypeId;
            if (request.BusinessTypeId.HasValue)
            {
                var businessType = await _unitOfWork.GetRepository<BusinessType>().GetByIdAsync(request.BusinessTypeId.Value);
                if (businessType == null || businessType.IsDeleted)
                    throw new ArgumentException("Business type not found or not available.");
                businessTypeId = businessType.Id;
            }
            else if (!string.IsNullOrWhiteSpace(request.NewBusinessTypeName))
            {
                var newType = new BusinessType
                {
                    Name = request.NewBusinessTypeName.Trim(),
                    Description = request.NewBusinessTypeDescription,
                    IsDeleted = true,
                    CreatedAt = DateTime.UtcNow
                };
                await _unitOfWork.GetRepository<BusinessType>().AddAsync(newType);
                await _unitOfWork.SaveChangesAsync();
                businessTypeId = newType.Id;
            }
            else
            {
                throw new ArgumentException("Business type is required.");
            }

            _mapper.Map(request, employer);

            if (employer.User != null)
            {
                _mapper.Map(request, employer.User);
                employer.User.UpdatedAt = DateTime.UtcNow;
                var result = await _userManager.UpdateAsync(employer.User);
                if (!result.Succeeded)
                    throw new Exception(string.Join("; ", result.Errors.Select(e => e.Description)));
            }

            employer.BusinessTypeId = businessTypeId;
            employer.UpdatedAt = DateTime.UtcNow;
            await repo.Update(employer);
            // Xử lý SocialLinks
            var socialRepo = _unitOfWork.GetRepository<SocialLink>();
            var oldLinks = await socialRepo.GetAllAsync(sl => sl.UserId == userId && !sl.IsDeleted);

            foreach (var oldLink in oldLinks)
            {
                await socialRepo.SoftDelete(oldLink);
            }

            if (request.SocialLinks != null && request.SocialLinks.Count > 0)
            {
                foreach (var link in request.SocialLinks)
                {
                    var entity = _mapper.Map<SocialLink>(link);
                    entity.UserId = userId;
                    await socialRepo.AddAsync(entity);
                }
            }
            await _unitOfWork.SaveChangesAsync();
            return await GetProfileAsync(userId);
        }
    }
}
