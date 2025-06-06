using AutoMapper;
using Repository.Entities;
using Repository.Interfaces;
using Service.Interfaces;
using Service.Models.SubscriptionModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepo<Subscription> _subscriptionRepo;
        private readonly IMapper _mapper;

        public SubscriptionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _subscriptionRepo = _unitOfWork.GetRepository<Subscription>();
            _mapper = mapper;
        }

        // Kiểm tra và cập nhật trạng thái hết hạn tự động
        private async Task CheckAndUpdateExpirationAsync(Subscription entity)
        {
            if (entity.IsActive && entity.EndDate < DateTime.UtcNow)
            {
                entity.IsActive = false;
                await _subscriptionRepo.Update(entity);
                await _unitOfWork.SaveChangesAsync();
            }
        }

        public async Task<List<SubscriptionModel>> GetAllAsync()
        {
            var list = await _subscriptionRepo.GetAllAsync();
            foreach (var entity in list)
            {
                await CheckAndUpdateExpirationAsync(entity);
            }
            list = await _subscriptionRepo.GetAllAsync();
            return _mapper.Map<List<SubscriptionModel>>(list);
        }

        public async Task<SubscriptionModel> GetByIdAsync(int id)
        {
            var entity = await _subscriptionRepo.GetByIdAsync(id);
            if (entity == null)
                throw new KeyNotFoundException("Subscription not found.");

            await CheckAndUpdateExpirationAsync(entity);
            return _mapper.Map<SubscriptionModel>(entity);
        }

        public async Task<SubscriptionModel> CreateAsync(CreateSubscriptionModel dto)
        {
            if (dto.StartDate >= dto.EndDate)
                throw new ArgumentException("StartDate must be before EndDate.");

            var entity = _mapper.Map<Subscription>(dto);
            entity.IsActive = true;
            await _subscriptionRepo.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<SubscriptionModel>(entity);
        }

        public async Task<SubscriptionModel> UpdateAsync(int id, UpdateSubscriptionModel dto)
        {
            var entity = await _subscriptionRepo.GetByIdAsync(id);
            if (entity == null)
                throw new KeyNotFoundException("Subscription not found.");

            if (dto.StartDate >= dto.EndDate)
                throw new ArgumentException("StartDate must be before EndDate.");

            _mapper.Map(dto, entity);
            await _subscriptionRepo.Update(entity);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<SubscriptionModel>(entity);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _subscriptionRepo.GetByIdAsync(id);
            if (entity == null)
                throw new KeyNotFoundException("Subscription not found.");

            entity.IsActive = false;
            await _subscriptionRepo.Update(entity);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RestoreAsync(int id)
        {
            var entity = await _subscriptionRepo.GetByIdAsync(id);
            if (entity == null)
                throw new KeyNotFoundException("Subscription not found.");

            entity.IsActive = true;
            await _subscriptionRepo.Update(entity);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> HardDeleteAsync(int id)
        {
            var entity = await _subscriptionRepo.GetByIdAsync(id);
            if (entity == null)
                throw new KeyNotFoundException("Subscription not found.");

            // Kiểm tra ràng buộc FK với JobPosting
            var jobPostings = await _unitOfWork.GetRepository<JobPosting>()
                .GetAllAsync(jp => jp.SubscriptionId == id && !jp.IsDeleted);
            if (jobPostings.Any())
                throw new InvalidOperationException("Cannot delete Subscription because there are related JobPostings.");

            await _subscriptionRepo.HardDelete(s => s.Id == id);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CanPostJobAsync(int subscriptionId)
        {
            var subscription = await _subscriptionRepo.GetByIdAsync(subscriptionId);
            if (subscription == null || !subscription.IsActive)
                return false;

            var package = await _unitOfWork.GetRepository<SubscriptionPackage>()
                .GetByIdAsync(subscription.SubscriptionPackageId);

            if (package == null)
                return false;

            if (package.JobPostLimit == int.MaxValue)
                return true;

            var jobPostings = await _unitOfWork.GetRepository<JobPosting>()
                .GetAllAsync(jp => jp.SubscriptionId == subscriptionId && !jp.IsDeleted);

            int usedPosts = jobPostings.Count();

            return usedPosts < package.JobPostLimit;
        }

        /// <summary>
        /// Mua gói mới. Nếu có gói active trước đó thì:
        /// - Nếu forceReplace = false => lỗi bắt confirm từ frontend.
        /// - Nếu forceReplace = true => tự hủy gói cũ rồi mua gói mới.
        /// </summary>
        public async Task<SubscriptionModel> PurchaseSubscriptionAsync(CreateSubscriptionModel dto, bool forceReplace = false)
        {
            var activeSubs = await _subscriptionRepo.GetAllAsync(s => s.EmployerId == dto.EmployerId && s.IsActive);

            if (activeSubs.Any())
            {
                if (!forceReplace)
                    throw new InvalidOperationException("Employer already has an active subscription. Please cancel it before purchasing a new one.");

                // Hủy soft các gói active trước khi mua
                foreach (var oldSub in activeSubs)
                {
                    oldSub.IsActive = false;
                    await _subscriptionRepo.Update(oldSub);
                }
                await _unitOfWork.SaveChangesAsync();
            }

            if (dto.StartDate >= dto.EndDate)
                throw new ArgumentException("StartDate must be before EndDate.");

            var entity = _mapper.Map<Subscription>(dto);
            entity.IsActive = true;
            await _subscriptionRepo.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<SubscriptionModel>(entity);
        }
    }
}
