using AutoMapper;
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

        public EmployerService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Employer> GetProfileAsync(string userId)
        {
            var employer = await _unitOfWork.GetRepository<Employer>().GetSingleByConditionAsynce(u => u.UserId == userId, t => t.BusinessType);
            if (employer == null)
                throw new KeyNotFoundException("Employer profile not found.");
            return employer;
        }

        public async Task<Employer> CreateProfileAsync(string userId, EmployerEditModel request)
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
            return employer;
        }

        public async Task<Employer> UpdateProfileAsync(string userId, EmployerEditModel request)
        {
            var repo = _unitOfWork.GetRepository<Employer>();
            var employer = await repo.GetSingleByConditionAsynce(e => e.UserId == userId);
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
            employer.BusinessTypeId = businessTypeId;
            employer.UpdatedAt = DateTime.UtcNow;
            await repo.Update(employer);
            await _unitOfWork.SaveChangesAsync();
            return employer;
        }
    }
}
