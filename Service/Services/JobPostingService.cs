using AutoMapper;
using Repository.Entities;
using Repository.Helpers;
using Repository.Interfaces;
using Service.Interfaces;
using Service.Models.JobPostings;

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

        public async Task<IEnumerable<JobPostingModel>> GetJobPostingsByEmployerAsync(int employerId)
        {
            var repo = _unitOfWork.GetRepository<JobPosting>();
            var result = await repo.GetAllAsync(
                j => j.EmployerId == employerId && !j.IsDeleted,
                j => j.JobPostingTags.Select(jpt => jpt.JobTag)
            );

            if (result == null || !result.Any())
            {
                return Enumerable.Empty<JobPostingModel>();
            }

            return _mapper.Map<IEnumerable<JobPostingModel>>(result);
        }

        public async Task<JobPostingModel?> GetJobPostingByIdAsync(int id)
        {
            var repo = _unitOfWork.GetRepository<JobPosting>();
            var entity = await repo.GetSingleByConditionAsynce(
                j => j.Id == id && !j.IsDeleted,
                j => j.JobPostingTags.Select(jpt => jpt.JobTag)
            );

            return entity == null ? null : _mapper.Map<JobPostingModel>(entity);
        }

        public async Task<JobPostingModel> CreateJobPostingAsync(CreateJobPostingModel model, List<int> tagIds)
        {
            var jobPostingRepo = _unitOfWork.GetRepository<JobPosting>();
            var jobPostingTagRepo = _unitOfWork.GetRepository<JobPostingTag>();

            try
            {
                var jobPosting = _mapper.Map<JobPosting>(model);

                jobPosting.PostedAt = DateTime.UtcNow;
                jobPosting.Status = "PENDING";
                jobPosting.IsDeleted = false;
                jobPosting.CreatedAt = DateTime.UtcNow;

                await jobPostingRepo.AddAsync(jobPosting);
                await _unitOfWork.SaveChangesAsync();

                // Gán tag cho bài đăng
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

    }
}