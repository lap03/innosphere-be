using AutoMapper;
using Repository.Entities;
using Repository.Interfaces;
using Service.Interfaces;
using Service.Models.SocialLinkModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Service.Services
{
    public class SocialLinkService : ISocialLinkService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SocialLinkService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // ✅ Get all social links of a specific user
        public async Task<List<SocialLinkModel>> GetByUserIdAsync(string userId)
        {
            var repo = _unitOfWork.GetRepository<SocialLink>();
            var list = await repo.GetAllAsync(sl => sl.UserId == userId && !sl.IsDeleted);
            return _mapper.Map<List<SocialLinkModel>>(list);
        }

        // ✅ Optional: Get all active social links (admin use or future use)
        public async Task<List<SocialLinkModel>> GetAllActiveAsync()
        {
            var repo = _unitOfWork.GetRepository<SocialLink>();
            var list = await repo.GetAllAsync(sl => !sl.IsDeleted);
            return _mapper.Map<List<SocialLinkModel>>(list);
        }

        // ✅ Get by ID
        public async Task<SocialLinkModel> GetByIdAsync(int id)
        {
            var repo = _unitOfWork.GetRepository<SocialLink>();
            var entity = await repo.GetByIdAsync(id);
            if (entity == null) throw new KeyNotFoundException("Social link not found.");
            return _mapper.Map<SocialLinkModel>(entity);
        }

        // ✅ Create a social link (auto-bind userId)
        public async Task<SocialLinkModel> CreateAsync(CreateSocialLinkModel dto, string userId)
        {
            var repo = _unitOfWork.GetRepository<SocialLink>();
            var entity = _mapper.Map<SocialLink>(dto);
            entity.UserId = userId;
            await repo.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<SocialLinkModel>(entity);
        }

        // ✅ Update by ID
        public async Task<SocialLinkModel> UpdateAsync(int id, UpdateSocialLinkModel dto)
        {
            var repo = _unitOfWork.GetRepository<SocialLink>();
            var entity = await repo.GetByIdAsync(id);
            if (entity == null) throw new KeyNotFoundException("Social link not found.");

            _mapper.Map(dto, entity);
            await repo.Update(entity);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<SocialLinkModel>(entity);
        }

        // ✅ Soft delete
        public async Task<bool> DeleteAsync(int id)
        {
            var repo = _unitOfWork.GetRepository<SocialLink>();
            var entity = await repo.GetByIdAsync(id);
            if (entity == null) throw new KeyNotFoundException("Social link not found.");

            await repo.SoftDelete(entity);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        // ✅ Restore soft-deleted record
        public async Task<bool> RestoreAsync(int id)
        {
            var repo = _unitOfWork.GetRepository<SocialLink>();
            var entity = await repo.GetByIdAsync(id);
            if (entity == null) throw new KeyNotFoundException("Social link not found.");

            entity.IsDeleted = false;
            await repo.Update(entity);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        // ✅ Hard delete (permanent)
        public async Task<bool> HardDeleteAsync(int id)
        {
            var repo = _unitOfWork.GetRepository<SocialLink>();
            var entity = await repo.GetByIdAsync(id);
            if (entity == null) throw new KeyNotFoundException("Social link not found.");

            await repo.HardDelete(sl => sl.Id == id);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
