using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Repository.Entities;
using Repository.Helpers;
using Repository.Interfaces;
using Service.Extensions;
using Service.Interfaces;
using Service.Models.JobPostings;
using Service.Models.JobTagModels;
using Service.Models.PagedResultModels;
using System.Linq.Expressions;

namespace Service.Services
{
    public class JobPostingService : IJobPostingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public JobPostingService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PagedResultModel<JobPostingModel>> GetJobPostingsAsync(JobPostingFilterModel filter)
        {
            var query = _unitOfWork.GetRepository<JobPosting>()
                .GetAllQueryable() // Sử dụng AsQueryable thay vì GetAll
                .ApplyBaseQuery()
                .FilterByCity(filter.CityId)
                .FilterByJobType(filter.JobType)
                .FilterByStatus(filter.Status)
                .FilterByHourlyRateRange(filter.MinHourlyRate, filter.MaxHourlyRate)
                .FilterByStartDate(filter.StartFrom)
                .FilterByEndDate(filter.EndTo)
                .FilterByKeyword(filter.Keyword)
                .ApplyDefaultSorting();

            // Get total count before paging
            var totalCount = await query.CountAsync();

            // Apply paging
            var data = await query
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();

            // Map to model
            var mappedData = _mapper.Map<IEnumerable<JobPostingModel>>(data);

            return new PagedResultModel<JobPostingModel>
            {
                Data = mappedData,
                Page = filter.Page,
                PageSize = filter.PageSize,
                TotalCount = totalCount
            };
        }

        public async Task<IEnumerable<JobPostingModel>> GetJobPostingsByEmployerAsync(int employerId, string? status = null)
        {
            var repo = _unitOfWork.GetRepository<JobPosting>();
            var jobTagRepo = _unitOfWork.GetRepository<JobTag>();

            // Build predicate based on parameters
            Expression<Func<JobPosting, bool>> predicate = j => j.EmployerId == employerId && !j.IsDeleted;
            if (!string.IsNullOrEmpty(status))
            {
                predicate = j => j.EmployerId == employerId && !j.IsDeleted && j.Status == status;
            }

            var result = await repo.GetAllAsync(
                predicate,
                j => j.JobPostingTags,
                j => j.City,
                j => j.Employer,
                j => j.Employer.BusinessType
            );

            if (result == null || !result.Any())
            {
                return Enumerable.Empty<JobPostingModel>();
            }

            var jobPostingModels = new List<JobPostingModel>();

            foreach (var entity in result)
            {
                var model = _mapper.Map<JobPostingModel>(entity);

                // Get JobTagIds from JobPostingTags
                var tagIds = entity.JobPostingTags?.Select(jpt => jpt.JobTagId).ToList() ?? new List<int>();

                // Get JobTag info from repo based on ids
                if (tagIds.Any())
                {
                    var jobTags = await jobTagRepo.GetAllAsync(jt => tagIds.Contains(jt.Id));
                    model.JobTags = jobTags.Select(jt => _mapper.Map<JobTagModel>(jt)).ToList();
                }
                else
                {
                    model.JobTags = new List<JobTagModel>();
                }

                jobPostingModels.Add(model);
            }

            return jobPostingModels;
        }

        public async Task<JobPostingModel?> GetJobPostingByIdAsync(int id)
        {
            var repo = _unitOfWork.GetRepository<JobPosting>();
            var jobTagRepo = _unitOfWork.GetRepository<JobTag>();
            var entity = await repo.GetSingleByConditionAsynce(
                j => j.Id == id && !j.IsDeleted,
                j => j.JobPostingTags,
                j => j.City,
                j => j.Employer,
                j => j.Employer.BusinessType
            );

            if (entity == null)
                return null;

            var model = _mapper.Map<JobPostingModel>(entity);

            // Lấy danh sách JobTagId từ JobPostingTags
            var tagIds = entity.JobPostingTags?.Select(jpt => jpt.JobTagId).ToList() ?? new List<int>();

            // Lấy thông tin JobTag từ repo dựa trên danh sách id
            if (tagIds.Any())
            {
                var jobTags = await jobTagRepo.GetAllAsync(jt => tagIds.Contains(jt.Id));
                model.JobTags = jobTags.Select(jt => _mapper.Map<JobTagModel>(jt)).ToList();
            }
            else
            {
                model.JobTags = new List<JobTagModel>();
            }

            return model;
        }

        public async Task<JobPostingModel> CreateJobPostingAsync(CreateJobPostingModel model, List<int> tagIds)
        {
            var jobPostingRepo = _unitOfWork.GetRepository<JobPosting>();
            var jobPostingTagRepo = _unitOfWork.GetRepository<JobPostingTag>();
            var subscriptionRepo = _unitOfWork.GetRepository<Subscription>();
            var packageRepo = _unitOfWork.GetRepository<SubscriptionPackage>();
            var jobRepo = _unitOfWork.GetRepository<JobPosting>();

            try
            {
                // 1. Lấy subscription active mới nhất của employer
                var subscription = (await subscriptionRepo.GetAllAsync(s => s.EmployerId == model.EmployerId && s.IsActive))
                                    .OrderByDescending(s => s.EndDate)
                                    .FirstOrDefault();

                if (subscription == null)
                    throw new InvalidOperationException("Employer has no active subscription.");

                // 2. Lấy gói subscription để kiểm tra giới hạn đăng tin
                var package = await packageRepo.GetByIdAsync(subscription.SubscriptionPackageId);
                if (package == null)
                    throw new InvalidOperationException("Subscription package not found.");

                // 3. Nếu giới hạn tin đăng không phải vô hạn, kiểm tra số lượng bài đã đăng
                if (package.JobPostLimit != int.MaxValue)
                {
                    var currentPostCount = await jobRepo.CountAsync(jp => jp.SubscriptionId == subscription.Id && !jp.IsDeleted);
                    if (currentPostCount >= package.JobPostLimit)
                        throw new InvalidOperationException("Job post limit reached for current subscription.");
                }
                var jobPosting = _mapper.Map<JobPosting>(model);

                jobPosting.PostedAt = DateTime.UtcNow;
                jobPosting.Status = "PENDING";
                jobPosting.IsDeleted = false;
                jobPosting.CreatedAt = DateTime.UtcNow;

                await jobPostingRepo.AddAsync(jobPosting);
                await _unitOfWork.SaveChangesAsync();

                // Gán tag cho bài đăng
                if (tagIds != null && tagIds.Any())
                {
                    foreach (var tagId in tagIds)
                    {
                        var jobPostingTag = new JobPostingTag
                        {
                            JobPostingId = jobPosting.Id,
                            JobTagId = tagId,
                            CreatedAt = DateTime.UtcNow,
                            IsDeleted = false
                        };
                        await jobPostingTagRepo.AddAsync(jobPostingTag);
                    }
                    await _unitOfWork.SaveChangesAsync();
                }

                return _mapper.Map<JobPostingModel>(jobPosting);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> UpdateJobPostingStatusAsync(int jobPostingId, string status)
        {
            var repo = _unitOfWork.GetRepository<JobPosting>();
            var jobPosting = await repo.GetByIdAsync(jobPostingId);

            if (jobPosting == null || jobPosting.IsDeleted)
                return false;

            // Chỉ cho phép các status hợp lệ
            var validStatuses = new[] { JobPostingStatusHelper.Approved, JobPostingStatusHelper.Rejected, JobPostingStatusHelper.Closed, JobPostingStatusHelper.Completed };
            if (!validStatuses.Contains(status))
                return false;

            jobPosting.Status = status;
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Admin: Lấy tất cả job posting từ tất cả employers
        /// </summary>
        public async Task<List<JobPostingModel>> GetAllForAdminAsync()
        {
            var repo = _unitOfWork.GetRepository<JobPosting>();
            var jobTagRepo = _unitOfWork.GetRepository<JobTag>();

            // Lấy tất cả job posting không bị xóa, bao gồm thông tin employer và city
            var list = await repo.GetAllAsync(
                jp => !jp.IsDeleted,
                jp => jp.JobPostingTags,
                jp => jp.City,
                jp => jp.Employer,
                jp => jp.Employer.BusinessType
            );

            var jobPostingModels = new List<JobPostingModel>();

            foreach (var entity in list)
            {
                var model = _mapper.Map<JobPostingModel>(entity);

                // Get JobTagIds from JobPostingTags
                var tagIds = entity.JobPostingTags?.Select(jpt => jpt.JobTagId).ToList() ?? new List<int>();

                // Get JobTag info from repo based on ids
                if (tagIds.Any())
                {
                    var jobTags = await jobTagRepo.GetAllAsync(jt => tagIds.Contains(jt.Id));
                    model.JobTags = jobTags.Select(jt => _mapper.Map<JobTagModel>(jt)).ToList();
                }
                else
                {
                    model.JobTags = new List<JobTagModel>();
                }

                jobPostingModels.Add(model);
            }

            return jobPostingModels;
        }

    }
}