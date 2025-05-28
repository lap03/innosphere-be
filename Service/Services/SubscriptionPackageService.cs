using AutoMapper;
using Repository.Entities;
using Repository.Interfaces;
using Service.Interfaces;
using Service.Models.SubscriptionPackageModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class SubscriptionPackageService : ISubscriptionPackageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SubscriptionPackageService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<SubscriptionPackageModel>> GetAllAsync()
        {
            var list = await _unitOfWork.GetRepository<SubscriptionPackage>().GetAllAsync();
            return _mapper.Map<List<SubscriptionPackageModel>>(list);
        }

        public async Task<SubscriptionPackageModel> GetByIdAsync(int id)
        {
            var entity = await _unitOfWork.GetRepository<SubscriptionPackage>().GetByIdAsync(id);
            if (entity == null)
                throw new KeyNotFoundException("Subscription package not found."); // Middleware sẽ xử lý lỗi này

            return _mapper.Map<SubscriptionPackageModel>(entity);
        }

        public async Task<SubscriptionPackageModel> CreateAsync(CreateSubscriptionPackageModel dto)
        {
            try
            {
                var entity = _mapper.Map<SubscriptionPackage>(dto);
                await _unitOfWork.GetRepository<SubscriptionPackage>().AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();
                return _mapper.Map<SubscriptionPackageModel>(entity);
            }
            catch (Exception ex)
            {
                // Middleware sẽ xử lý lỗi này và trả lỗi JSON
                throw new Exception($"Failed to create subscription package: {ex.Message}");
            }
        }

        public async Task<SubscriptionPackageModel> UpdateAsync(int id, UpdateSubscriptionPackageModel dto)
        {
            var repo = _unitOfWork.GetRepository<SubscriptionPackage>();
            var entity = await repo.GetByIdAsync(id);
            if (entity == null)
                throw new KeyNotFoundException("Subscription package not found."); // Middleware sẽ bắt lỗi 404

            try
            {
                _mapper.Map(dto, entity);
                await repo.Update(entity);
                await _unitOfWork.SaveChangesAsync();
                return _mapper.Map<SubscriptionPackageModel>(entity);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to update subscription package: {ex.Message}"); // middleware xử lý
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var repo = _unitOfWork.GetRepository<SubscriptionPackage>();
            var entity = await repo.GetByIdAsync(id);
            if (entity == null)
                throw new KeyNotFoundException("Subscription package not found."); //middleware xử lý lỗi không tìm thấy

            try
            {
                await repo.SoftDelete(entity);
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to delete subscription package: {ex.Message}"); // middleware xử lý lỗi xoá
            }
        }
    }
}
