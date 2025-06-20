using AutoMapper;
using Repository.Entities;
using Repository.Interfaces;
using Service.Interfaces;
using Service.Models.SubscriptionModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Services
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SubscriptionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        private async Task UpdateExpirationStatusAsync(Subscription sub)
        {
            if (sub.IsActive && sub.EndDate < DateTime.UtcNow)
            {
                sub.IsActive = false;
                var repo = _unitOfWork.GetRepository<Subscription>();
                await repo.Update(sub);
                await _unitOfWork.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Lấy tất cả subscription của employer, cập nhật trạng thái hết hạn nếu cần.
        /// </summary>
        public async Task<List<SubscriptionModel>> GetAllByEmployerAsync(int employerId)
        {
            var repo = _unitOfWork.GetRepository<Subscription>();

            var subscriptions = await repo.GetAllAsync(s => s.EmployerId == employerId);

            foreach (var sub in subscriptions)
            {
                await UpdateExpirationStatusAsync(sub);
            }

            subscriptions = await repo.GetAllAsync(s => s.EmployerId == employerId);
            return _mapper.Map<List<SubscriptionModel>>(subscriptions);
        }

        /// <summary>
        /// Lấy subscription theo Id, cập nhật trạng thái hết hạn nếu cần.
        /// </summary>
        public async Task<SubscriptionModel> GetByIdAsync(int id)
        {
            var repo = _unitOfWork.GetRepository<Subscription>();
            var entity = await repo.GetByIdAsync(id);
            if (entity == null)
                throw new KeyNotFoundException("Subscription not found.");

            await UpdateExpirationStatusAsync(entity);

            return _mapper.Map<SubscriptionModel>(entity);
        }

        /// <summary>
        /// Mua gói mới, tự động hủy các gói cũ active.
        /// </summary>
        public async Task<SubscriptionModel> PurchaseSubscriptionAsync(CreateSubscriptionModel dto)
        {
            if (dto.StartDate >= dto.EndDate)
                throw new ArgumentException("StartDate must be before EndDate.");

            var repo = _unitOfWork.GetRepository<Subscription>();

            var activeSubs = await repo.GetAllAsync(s => s.EmployerId == dto.EmployerId && s.IsActive);
            foreach (var sub in activeSubs)
            {
                sub.IsActive = false;
                await repo.Update(sub);
            }
            await _unitOfWork.SaveChangesAsync();

            var entity = _mapper.Map<Subscription>(dto);
            entity.IsActive = true;
            await repo.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<SubscriptionModel>(entity);
        }

        /// <summary>
        /// Kiểm tra xem employer còn quyền đăng tin hay không.
        /// </summary>
        public async Task<bool> CanPostJobAsync(int employerId)
        {
            var repo = _unitOfWork.GetRepository<Subscription>();

            var subscription = (await repo.GetAllAsync(s => s.EmployerId == employerId && s.IsActive))
                .OrderByDescending(s => s.EndDate)
                .FirstOrDefault();

            if (subscription == null)
                return false;

            var packageRepo = _unitOfWork.GetRepository<SubscriptionPackage>();
            var package = await packageRepo.GetByIdAsync(subscription.SubscriptionPackageId);
            if (package == null)
                return false;

            if (package.JobPostLimit == int.MaxValue)
                return true;

            var jobRepo = _unitOfWork.GetRepository<JobPosting>();
            var currentPosts = await jobRepo.GetAllAsync(jp => jp.SubscriptionId == subscription.Id && !jp.IsDeleted);

            return currentPosts.Count() < package.JobPostLimit;
        }

        /// <summary>
        /// Hủy subscription (soft delete).
        /// </summary>
        public async Task<bool> CancelSubscriptionAsync(int subscriptionId, int employerId)
        {
            var repo = _unitOfWork.GetRepository<Subscription>();
            var subscription = await repo.GetByIdAsync(subscriptionId);
            if (subscription == null || subscription.EmployerId != employerId)
                throw new KeyNotFoundException("Subscription not found or does not belong to employer.");

            subscription.IsActive = false;
            await repo.Update(subscription);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Xóa cứng subscription nếu không còn tin đăng liên quan.
        /// </summary>
        public async Task<bool> HardDeleteAsync(int subscriptionId, int employerId)
        {
            var repo = _unitOfWork.GetRepository<Subscription>();
            var subscription = await repo.GetByIdAsync(subscriptionId);
            if (subscription == null || subscription.EmployerId != employerId)
                throw new KeyNotFoundException("Subscription not found or does not belong to employer.");

            var jobRepo = _unitOfWork.GetRepository<JobPosting>();
            var relatedJobs = await jobRepo.GetAllAsync(jp => jp.SubscriptionId == subscriptionId && !jp.IsDeleted);
            if (relatedJobs.Any())
                throw new InvalidOperationException("Cannot hard delete subscription with active job postings.");

            await repo.HardDelete(s => s.Id == subscriptionId);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Admin: Lấy tất cả subscription từ tất cả employers
        /// </summary>
        public async Task<List<SubscriptionModel>> GetAllForAdminAsync()
        {
            var repo = _unitOfWork.GetRepository<Subscription>();

            // Lấy tất cả subscription, bao gồm thông tin employer, user và package
            var subscriptions = await repo.GetAllAsync(
                null, // No filter, get all
                s => s.Employer,
                s => s.Employer.User,
                s => s.SubscriptionPackage
            );

            // Cập nhật trạng thái hết hạn cho từng subscription
            foreach (var sub in subscriptions)
            {
                await UpdateExpirationStatusAsync(sub);
            }

            // Lấy lại danh sách đã cập nhật trạng thái
            subscriptions = await repo.GetAllAsync(
                null, // No filter, get all
                s => s.Employer,
                s => s.Employer.User,
                s => s.SubscriptionPackage
            );

            return _mapper.Map<List<SubscriptionModel>>(subscriptions);
        }
    }
}
