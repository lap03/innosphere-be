using AutoMapper;
using Repository.Entities;
using Repository.Interfaces;
using Service.Interfaces;
using Service.Models.AdvertisementPackageModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Services
{
    public class AdvertisementPackageService : IAdvertisementPackageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepo<AdvertisementPackage> _advertisementPackageRepo;
        private readonly IMapper _mapper;

        public AdvertisementPackageService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _advertisementPackageRepo = _unitOfWork.GetRepository<AdvertisementPackage>();
            _mapper = mapper;
        }

        public async Task<List<AdvertisementPackageModel>> GetAllAsync()
        {
            var list = await _advertisementPackageRepo.GetAllAsync();
            return _mapper.Map<List<AdvertisementPackageModel>>(list);
        }

        public async Task<List<AdvertisementPackageModel>> GetAllActiveAsync()
        {
            var list = await _advertisementPackageRepo.GetAllAsync(ap => !ap.IsDeleted);
            return _mapper.Map<List<AdvertisementPackageModel>>(list);
        }

        public async Task<AdvertisementPackageModel> GetByIdAsync(int id)
        {
            var entity = await _advertisementPackageRepo.GetByIdAsync(id);
            if (entity == null) throw new KeyNotFoundException("Advertisement package not found.");
            return _mapper.Map<AdvertisementPackageModel>(entity);
        }

        public async Task<AdvertisementPackageModel> CreateAsync(CreateAdvertisementPackageModel dto)
        {
            try
            {
                var entity = _mapper.Map<AdvertisementPackage>(dto);
                await _advertisementPackageRepo.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();
                return _mapper.Map<AdvertisementPackageModel>(entity);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to create advertisement package: {ex.Message}");
            }
        }

        public async Task<AdvertisementPackageModel> UpdateAsync(int id, UpdateAdvertisementPackageModel dto)
        {
            var entity = await _advertisementPackageRepo.GetByIdAsync(id);
            if (entity == null) throw new KeyNotFoundException("Advertisement package not found.");

            try
            {
                _mapper.Map(dto, entity);
                await _advertisementPackageRepo.Update(entity);
                await _unitOfWork.SaveChangesAsync();
                return _mapper.Map<AdvertisementPackageModel>(entity);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to update advertisement package: {ex.Message}");
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _advertisementPackageRepo.GetByIdAsync(id);
            if (entity == null) throw new KeyNotFoundException("Advertisement package not found.");

            try
            {
                var ads = await _unitOfWork.GetRepository<Advertisement>()
                    .GetAllAsync(ad => ad.AdvertisementPackageId == id && !ad.IsDeleted);
                if (ads.Any())
                    throw new InvalidOperationException("Cannot delete AdvertisementPackage because it has active advertisements.");

                await _advertisementPackageRepo.HardDelete(ap => ap.Id == id);
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to delete advertisement package: {ex.Message}");
            }
        }

        public async Task<bool> RestoreAsync(int id)
        {
            var entity = await _advertisementPackageRepo.GetByIdAsync(id);
            if (entity == null) throw new KeyNotFoundException("Advertisement package not found.");

            try
            {
                entity.IsDeleted = false;
                await _advertisementPackageRepo.Update(entity);
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to restore advertisement package: {ex.Message}");
            }
        }

        public async Task<bool> HardDeleteAsync(int id)
        {
            var entity = await _advertisementPackageRepo.GetByIdAsync(id);
            if (entity == null) throw new KeyNotFoundException("Advertisement package not found.");

            var ads = await _unitOfWork.GetRepository<Advertisement>()
                .GetAllAsync(ad => ad.AdvertisementPackageId == id && !ad.IsDeleted);
            if (ads.Any())
                throw new InvalidOperationException("Cannot hard delete AdvertisementPackage because it has active advertisements.");

            try
            {
                await _advertisementPackageRepo.HardDelete(ap => ap.Id == id);
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to hard delete advertisement package: {ex.Message}");
            }
        }
    }
}
