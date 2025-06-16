using AutoMapper;
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
        public async Task<JobApplicationModel> ApplyAsync(CreateJobApplicationModel model, string userId) // ✅ SỬA: userId là string
        {
            var repo = _unitOfWork.GetRepository<JobApplication>();

            bool alreadyApplied = await repo.AnyAsync(j =>
                j.JobPostingId == model.JobPostingId &&
                j.Worker.UserId == userId && // ✅ SỬA: so sánh string
                j.Status != "REJECTED");

            if (alreadyApplied)
                throw new InvalidOperationException("You have already applied for this job.");

            var entity = _mapper.Map<JobApplication>(model);
            entity.AppliedAt = DateTime.UtcNow;
            entity.Status = "PENDING";

            var workerRepo = _unitOfWork.GetRepository<Worker>();
            entity.Worker = await workerRepo.GetSingleByConditionAsynce(w => w.UserId == userId); // ✅ SỬA

            if (entity.Worker == null)
                throw new InvalidOperationException("Worker not found.");

            await repo.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<JobApplicationModel>(entity);
        }

        // Employer xem các đơn ứng tuyển vào job của mình
        public async Task<IEnumerable<JobApplicationModel>> GetByEmployerAsync(string userId, int? jobPostingId = null) // ✅ SỬA: userId là string
        {
            var jobRepo = _unitOfWork.GetRepository<JobApplication>();

            var query = await jobRepo.GetAllAsync(j =>
                j.JobPosting.Employer.UserId == userId && // ✅ SỬA: so sánh string
                !j.JobPosting.IsDeleted &&
                (jobPostingId == null || j.JobPostingId == jobPostingId),
                j => j.JobPosting, j => j.Worker, j => j.Resume);

            //if (query == null || !query.Any())
            //    throw new InvalidOperationException("Không có đơn ứng tuyển nào.");

            return _mapper.Map<IEnumerable<JobApplicationModel>>(query);
        }

        // Worker xem tất cả đơn ứng tuyển của mình
        public async Task<IEnumerable<JobApplicationModel>> GetByWorkerAsync(string userId) // ✅ SỬA
        {
            var repo = _unitOfWork.GetRepository<JobApplication>();

            var query = await repo.GetAllAsync(j =>
                j.Worker.UserId == userId, // ✅ SỬA
                j => j.JobPosting, j => j.Resume);

            return _mapper.Map<IEnumerable<JobApplicationModel>>(query);
        }

        // Lấy chi tiết đơn ứng tuyển
        public async Task<JobApplicationModel> GetByIdAsync(int id)
        {
            var repo = _unitOfWork.GetRepository<JobApplication>();

            var entity = await repo.GetSingleByConditionAsynce(j => j.Id == id,
                j => j.JobPosting, j => j.Resume, j => j.Worker);

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
        public async Task<bool> CancelApplicationAsync(int id, string userId) // ✅ SỬA
        {
            var repo = _unitOfWork.GetRepository<JobApplication>();

            var entity = await repo.GetByIdAsync(id);
            if (entity == null)
                throw new KeyNotFoundException("Job application not found.");

            if (entity.Worker.UserId != userId) // ✅ SỬA: so sánh string
                throw new UnauthorizedAccessException("You can only cancel your own applications.");

            entity.Status = "REJECTED";
            await repo.Update(entity);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
