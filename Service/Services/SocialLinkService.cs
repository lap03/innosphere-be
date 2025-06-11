using AutoMapper;
using Repository.Entities;
using Repository.Interfaces;
using Service.Interfaces;
using Service.Models.SocialLinkModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Services
{
    public class SocialLinkService : ISocialLinkService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepo<SocialLink> _repo;
        private readonly IMapper _mapper;

        public SocialLinkService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _repo = _unitOfWork.GetRepository<SocialLink>();
            _mapper = mapper;
        }

        public async Task<List<SocialLinkModel>> GetAllAsync()
        {
            var list = await _repo.GetAllAsync();
            return _mapper.Map<List<SocialLinkModel>>(list);
        }

        public async Task<List<SocialLinkModel>> GetAllActiveAsync()
        {
            var list = await _repo.GetAllAsync(sl => !sl.IsDeleted);
            return _mapper.Map<List<SocialLinkModel>>(list);
        }

        public async Task<SocialLinkModel> GetByIdAsync(int id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) throw new KeyNotFoundException("Social link not found.");
            return _mapper.Map<SocialLinkModel>(entity);
        }
        //FE có thể gọi /sociallinks/user/{userId} → lấy List các SocialLink của User.
        public async Task<List<SocialLinkModel>> GetByUserIdAsync(string userId)
        {
            var list = await _repo.GetAllAsync(sl => sl.UserId == userId && !sl.IsDeleted);
            return _mapper.Map<List<SocialLinkModel>>(list);
        }


        public async Task<SocialLinkModel> CreateAsync(CreateSocialLinkModel dto)
        {
            try
            {
                var entity = _mapper.Map<SocialLink>(dto);
                await _repo.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();
                return _mapper.Map<SocialLinkModel>(entity);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to create social link: {ex.Message}");
            }
        }

        public async Task<SocialLinkModel> UpdateAsync(int id, UpdateSocialLinkModel dto)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) throw new KeyNotFoundException("Social link not found.");

            try
            {
                _mapper.Map(dto, entity);
                await _repo.Update(entity);
                await _unitOfWork.SaveChangesAsync();
                return _mapper.Map<SocialLinkModel>(entity);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to update social link: {ex.Message}");
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) throw new KeyNotFoundException("Social link not found.");

            try
            {
                await _repo.SoftDelete(entity);
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to soft delete social link: {ex.Message}");
            }
        }

        public async Task<bool> RestoreAsync(int id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) throw new KeyNotFoundException("Social link not found.");

            try
            {
                entity.IsDeleted = false;
                await _repo.Update(entity);
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to restore social link: {ex.Message}");
            }
        }

        public async Task<bool> HardDeleteAsync(int id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) throw new KeyNotFoundException("Social link not found.");

            // TODO: check FK nếu có

            try
            {
                await _repo.HardDelete(sl => sl.Id == id);
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to hard delete social link: {ex.Message}");
            }
        }
    }
}
