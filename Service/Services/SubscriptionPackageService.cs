using AutoMapper;
using Repository.Entities;
using Repository.Interfaces;
using Service.Interfaces;
using Service.Models.SubscriptionPackageModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Services
{
    public class SubscriptionPackageService : ISubscriptionPackageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepo<SubscriptionPackage> _subscriptionPackageRepo;
        private readonly IMapper _mapper;

        public SubscriptionPackageService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _subscriptionPackageRepo = _unitOfWork.GetRepository<SubscriptionPackage>();
            _mapper = mapper;
        }

        public async Task<List<SubscriptionPackageModel>> GetAllAsync()
        {
            var list = await _subscriptionPackageRepo.GetAllAsync();
            return _mapper.Map<List<SubscriptionPackageModel>>(list);
        }

        public async Task<List<SubscriptionPackageModel>> GetAllActiveAsync()
        {
            var list = await _subscriptionPackageRepo.GetAllAsync(sp => !sp.IsDeleted);
            return _mapper.Map<List<SubscriptionPackageModel>>(list);
        }

        public async Task<SubscriptionPackageModel> GetByIdAsync(int id)
        {
            var entity = await _subscriptionPackageRepo.GetByIdAsync(id);
            if (entity == null) throw new KeyNotFoundException("Subscription package not found.");
            return _mapper.Map<SubscriptionPackageModel>(entity);
        }

        public async Task<SubscriptionPackageModel> CreateAsync(CreateSubscriptionPackageModel dto)
        {
            try
            {
                var entity = _mapper.Map<SubscriptionPackage>(dto);
                await _subscriptionPackageRepo.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();
                return _mapper.Map<SubscriptionPackageModel>(entity);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to create subscription package: {ex.Message}");
            }
        }

        public async Task<SubscriptionPackageModel> UpdateAsync(int id, UpdateSubscriptionPackageModel dto)
        {
            var entity = await _subscriptionPackageRepo.GetByIdAsync(id);
            if (entity == null) throw new KeyNotFoundException("Subscription package not found.");

            try
            {
                _mapper.Map(dto, entity);
                await _subscriptionPackageRepo.Update(entity);
                await _unitOfWork.SaveChangesAsync();
                return _mapper.Map<SubscriptionPackageModel>(entity);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to update subscription package: {ex.Message}");
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _subscriptionPackageRepo.GetByIdAsync(id);
            if (entity == null) throw new KeyNotFoundException("Subscription package not found.");

            try
            {
                // Kiểm tra ràng buộc nếu cần, ví dụ bảng Subscription (nếu có FK)
                var subscriptions = await _unitOfWork.GetRepository<Subscription>()
                    .GetAllAsync(s => s.SubscriptionPackageId == id && !s.IsDeleted);
                if (subscriptions.Any())
                    throw new InvalidOperationException("Cannot delete SubscriptionPackage because it has active subscriptions.");

                await _subscriptionPackageRepo.HardDelete(sp => sp.Id == id);
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to delete subscription package: {ex.Message}");
            }
        }

        public async Task<bool> RestoreAsync(int id)
        {
            var entity = await _subscriptionPackageRepo.GetByIdAsync(id);
            if (entity == null) throw new KeyNotFoundException("Subscription package not found.");

            try
            {
                entity.IsDeleted = false;
                await _subscriptionPackageRepo.Update(entity);
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to restore subscription package: {ex.Message}");
            }
        }

        public async Task<bool> HardDeleteAsync(int id)
        {
            var entity = await _subscriptionPackageRepo.GetByIdAsync(id);
            if (entity == null) throw new KeyNotFoundException("Subscription package not found.");

            var subscriptions = await _unitOfWork.GetRepository<Subscription>()
                .GetAllAsync(s => s.SubscriptionPackageId == id && !s.IsDeleted);
            if (subscriptions.Any())
                throw new InvalidOperationException("Cannot hard delete SubscriptionPackage because it has active subscriptions.");

            try
            {
                await _subscriptionPackageRepo.HardDelete(sp => sp.Id == id);
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to hard delete subscription package: {ex.Message}");
            }
        }
    }
}
