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
        public async Task<JobApplicationModel> ApplyAsync(CreateJobApplicationModel model, int workerId)
        {
            var repo = _unitOfWork.GetRepository<JobApplication>();

            // Kiểm tra đã nộp đơn cho job này chưa
            bool alreadyApplied = await repo.AnyAsync(j =>
                j.JobPostingId == model.JobPostingId &&
                j.Worker.UserId == workerId.ToString() &&
                j.Status != "REJECTED");

            if (alreadyApplied)
                throw new InvalidOperationException("You have already applied for this job.");

            var entity = _mapper.Map<JobApplication>(model);
            entity.AppliedAt = DateTime.UtcNow;
            entity.Status = "PENDING";
            entity.Worker = await _unitOfWork.GetRepository<Worker>()
                .GetSingleByConditionAsynce(w => w.UserId == workerId.ToString());

            if (entity.Worker == null)
                throw new InvalidOperationException("Worker not found.");

            await repo.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<JobApplicationModel>(entity);
        }

        // Employer xem các đơn ứng tuyển vào job của mình, lọc theo jobPostingId
        public async Task<IEnumerable<JobApplicationModel>> GetByEmployerAsync(int employerId, int? jobPostingId = null)
        {
            var repo = _unitOfWork.GetRepository<JobApplication>();

            var query = await repo.GetAllAsync(j =>
                j.JobPosting.EmployerId == employerId &&
                !j.JobPosting.IsDeleted &&
                (jobPostingId == null || j.JobPostingId == jobPostingId),
                j => j.JobPosting, j => j.Worker, j => j.Resume);

            return _mapper.Map<IEnumerable<JobApplicationModel>>(query);
        }

        // Worker xem tất cả đơn ứng tuyển của mình
        public async Task<IEnumerable<JobApplicationModel>> GetByWorkerAsync(int workerId)
        {
            var repo = _unitOfWork.GetRepository<JobApplication>();

            var query = await repo.GetAllAsync(j =>
                j.Worker.UserId == workerId.ToString(),
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

        // Worker hủy đơn ứng tuyển (đổi status sang REJECTED)
        public async Task<bool> CancelApplicationAsync(int id, int workerId)
        {
            var repo = _unitOfWork.GetRepository<JobApplication>();

            var entity = await repo.GetByIdAsync(id);
            if (entity == null)
                throw new KeyNotFoundException("Job application not found.");

            if (entity.Worker.UserId != workerId.ToString())
                throw new UnauthorizedAccessException("You can only cancel your own applications.");

            entity.Status = "REJECTED";
            await repo.Update(entity);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
