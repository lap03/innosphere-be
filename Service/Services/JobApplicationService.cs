using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Repository.Entities;
using Repository.Interfaces;
using Service.Interfaces;
using Service.Models.JobApplicationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Services
{
    public class JobApplicationService : IJobApplicationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public JobApplicationService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // Worker nộp đơn ứng tuyển
        public async Task<JobApplicationModel> ApplyAsync(CreateJobApplicationModel model, string userId)
        {
            if (!model.ResumeId.HasValue)
                throw new InvalidOperationException("ResumeId is required.");

            var workerRepo = _unitOfWork.GetRepository<Worker>();
            var worker = await workerRepo.GetSingleByConditionAsynce(w => w.UserId == userId, w => w.User); // ✅ Include User để Worker.User.FullName hoạt động

            if (worker == null)
                throw new InvalidOperationException("Worker not found.");

            var resumeRepo = _unitOfWork.GetRepository<Resume>();
            var resume = await resumeRepo.GetByIdAsync(model.ResumeId.Value);
            if (resume == null || resume.WorkerId != worker.Id)
                throw new InvalidOperationException("Resume not found or does not belong to this user.");

            var jobPostingRepo = _unitOfWork.GetRepository<JobPosting>();
            var jobPosting = await jobPostingRepo.GetSingleByConditionAsynce(j => j.Id == model.JobPostingId, j => j.Employer);
            if (jobPosting == null)
                throw new InvalidOperationException("Job posting not found.");

            var jobAppRepo = _unitOfWork.GetRepository<JobApplication>();
            bool alreadyApplied = await jobAppRepo.AnyAsync(j =>
                j.JobPostingId == model.JobPostingId &&
                j.Worker.UserId == userId &&
                j.Status != "REJECTED");

            if (alreadyApplied)
                throw new InvalidOperationException("You have already applied for this job.");

            var entity = new JobApplication
            {
                JobPostingId = model.JobPostingId,
                ResumeId = model.ResumeId.Value,
                CoverNote = model.CoverNote,
                AppliedAt = DateTime.UtcNow,
                Status = "PENDING",
                Worker = worker,
                Employer = jobPosting.Employer // ✅ đúng ý nghĩa nghiệp vụ
            };

            await jobAppRepo.AddAsync(entity);

            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("❌ DbSave failed: " + (ex.InnerException?.Message ?? ex.Message));
            }

            return _mapper.Map<JobApplicationModel>(entity);
        }


        // Employer xem các đơn ứng tuyển vào job của mình
        public async Task<IEnumerable<JobApplicationModel>> GetByEmployerAsync(string userId, int? jobPostingId = null)
        {
            var jobRepo = _unitOfWork.GetRepository<JobApplication>();

            var query = await jobRepo.GetAllAsync(j =>
                  j.JobPosting.Employer.UserId == userId &&
                  !j.JobPosting.IsDeleted &&
                  (jobPostingId == null || j.JobPostingId == jobPostingId),
                  j => j.JobPosting,
                  j => j.JobPosting.Employer,
                  j => j.JobPosting.Employer.BusinessType,
                  j => j.JobPosting.City,
                  j => j.Resume,
                  j => j.Worker,
                  j => j.Worker.User);

            return _mapper.Map<IEnumerable<JobApplicationModel>>(query);
        }

        // Worker xem tất cả đơn ứng tuyển của mình
        public async Task<IEnumerable<JobApplicationModel>> GetByWorkerAsync(string userId)
        {
            var repo = _unitOfWork.GetRepository<JobApplication>();
            var query = await repo.GetAllAsync(j =>
                j.Worker.UserId == userId,
                j => j.JobPosting,
                j => j.JobPosting.Employer,
                j => j.JobPosting.Employer.BusinessType,
                j => j.JobPosting.City,
                j => j.Resume,
                j => j.Worker,
                j => j.Worker.User);

            return _mapper.Map<IEnumerable<JobApplicationModel>>(query);
        }

        // Lấy chi tiết đơn ứng tuyển
        public async Task<JobApplicationModel> GetByIdAsync(int id)
        {
            var repo = _unitOfWork.GetRepository<JobApplication>();

            var entity = await repo.GetSingleByConditionAsynce(j => j.Id == id,
                j => j.JobPosting,
                j => j.JobPosting.Employer,
                j => j.JobPosting.Employer.BusinessType,
                j => j.JobPosting.City,
                j => j.Resume,
                j => j.Worker,
                j => j.Worker.User);

            if (entity == null)
                throw new KeyNotFoundException("Job application not found.");

            return _mapper.Map<JobApplicationModel>(entity);
        }

        // Employer/Admin cập nhật trạng thái đơn
        public async Task<bool> UpdateStatusAsync(int id, UpdateJobApplicationStatusModel model)
        {
            var repo = _unitOfWork.GetRepository<JobApplication>();

            var entity = await repo.GetByIdAsync(id);
            if (entity == null)
                throw new KeyNotFoundException("Job application not found.");

            entity.Status = model.Status;
            await repo.Update(entity);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        // Worker hủy đơn ứng tuyển
        public async Task<bool> CancelApplicationAsync(int id, string userId)
        {
            var repo = _unitOfWork.GetRepository<JobApplication>();

            var entity = await repo.GetByIdAsync(id);
            if (entity == null)
                throw new KeyNotFoundException("Job application not found.");

            if (entity.Worker.UserId != userId)
                throw new UnauthorizedAccessException("You can only cancel your own applications.");

            entity.Status = "REJECTED";
            await repo.Update(entity);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
