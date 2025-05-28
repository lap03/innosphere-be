using AutoMapper;
using Repository.Entities;
using Repository.Interfaces;
using Service.Interfaces;
using Service.Models.AdvertisementPackageModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class AdvertisementPackageService : IAdvertisementPackageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AdvertisementPackageService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<AdvertisementPackageModel>> GetAllAsync()
        {
            var packages = await _unitOfWork.GetRepository<AdvertisementPackage>().GetAllAsync();
            return _mapper.Map<List<AdvertisementPackageModel>>(packages);
        }

        public async Task<AdvertisementPackageModel> GetByIdAsync(int id)
        {
            var package = await _unitOfWork.GetRepository<AdvertisementPackage>().GetByIdAsync(id);
            if (package == null)
                throw new KeyNotFoundException("Advertisement package not found."); // Middleware xử lý lỗi 404

            return _mapper.Map<AdvertisementPackageModel>(package);
        }

        public async Task<AdvertisementPackageModel> CreateAsync(CreateAdvertisementPackageModel dto)
        {
            try
            {
                var package = _mapper.Map<AdvertisementPackage>(dto);
                await _unitOfWork.GetRepository<AdvertisementPackage>().AddAsync(package);
                await _unitOfWork.SaveChangesAsync();
                return _mapper.Map<AdvertisementPackageModel>(package);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to create advertisement package: {ex.Message}"); // Middleware xử lý
            }
        }

        public async Task<AdvertisementPackageModel> UpdateAsync(int id, UpdateAdvertisementPackageModel dto)
        {
            var repo = _unitOfWork.GetRepository<AdvertisementPackage>();
            var package = await repo.GetByIdAsync(id);
            if (package == null)
                throw new KeyNotFoundException("Advertisement package not found."); // Middleware xử lý lỗi 404

            try
            {
                _mapper.Map(dto, package);
                await repo.Update(package);
                await _unitOfWork.SaveChangesAsync();
                return _mapper.Map<AdvertisementPackageModel>(package);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to update advertisement package: {ex.Message}"); // Middleware xử lý
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var repo = _unitOfWork.GetRepository<AdvertisementPackage>();
            var package = await repo.GetByIdAsync(id);
            if (package == null)
                throw new KeyNotFoundException("Advertisement package not found."); // Middleware xử lý

            try
            {
                await repo.SoftDelete(package);
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to delete advertisement package: {ex.Message}"); // Middleware xử lý
            }
        }
    }
}
