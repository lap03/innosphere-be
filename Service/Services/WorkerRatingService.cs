﻿using AutoMapper;
using Repository.Entities;
using Repository.Interfaces;
using Service.Interfaces;
using Service.Models.WorkerRatingCriteriaModels;
using Service.Models.WorkerRatingModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class WorkerRatingService : IWorkerRatingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public WorkerRatingService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<WorkerRatingModel>> GetAllRatingsByWorkerAsync(int workerId)
        {
            var ratings = await _unitOfWork.GetRepository<WorkerRating>()
                .GetAllAsync(r => r.WorkerId == workerId && !r.IsDeleted);

            return _mapper.Map<IEnumerable<WorkerRatingModel>>(ratings);
        }

        public async Task<IEnumerable<WorkerRatingCriteriaModel>> GetRatingDetailsAsync(int workerRatingId)
        {
            var details = await _unitOfWork.GetRepository<WorkerRatingCriteria>()
                .GetAllAsync(d => d.WorkerRatingId == workerRatingId && !d.IsDeleted);

            return _mapper.Map<IEnumerable<WorkerRatingCriteriaModel>>(details);
        }

        public async Task CreateWorkerRatingAsync(CreateWorkerRatingModel model)
        {
            // 1. Kiểm tra JobApplication tồn tại và đã ACCEPTED
            var jobAppRepo = _unitOfWork.GetRepository<JobApplication>();
            var jobApplication = await jobAppRepo.GetByIdAsync(model.JobApplicationId, j => j.JobPosting);
            if (jobApplication == null)
                throw new KeyNotFoundException("Job application not found.");
            if (!string.Equals(jobApplication.Status, "ACCEPTED", StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("Job application must be ACCEPTED to rate.");

            // 2. Kiểm tra JobPosting đã COMPLETED
            var jobPosting = jobApplication.JobPosting;
            if (jobPosting == null)
            {
                // Nếu navigation property chưa được include, lấy lại từ repo
                jobPosting = await _unitOfWork.GetRepository<JobPosting>().GetByIdAsync(jobApplication.JobPostingId);
            }
            if (jobPosting == null)
                throw new KeyNotFoundException("Job posting not found.");
            if (!string.Equals(jobPosting.Status, "COMPLETED", StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(jobPosting.Status, "CLOSED", StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("Job posting must be COMPLETED or CLOSED to rate.");

            // 3. Kiểm tra Worker tồn tại
            var workerRepository = _unitOfWork.GetRepository<Worker>();
            var existingWorker = await workerRepository.GetByIdAsync(model.WorkerId);
            if (existingWorker == null)
                throw new KeyNotFoundException("Worker not found.");

            // 4. Kiểm tra chi tiết đánh giá
            if (model.Details == null || !model.Details.Any())
                throw new ArgumentException("Details are required.");

            // 5. Kiểm tra tất cả RatingCriteria tồn tại
            var ratingCriteriaRepo = _unitOfWork.GetRepository<RatingCriteria>();
            foreach (var detail in model.Details)
            {
                var criteria = await ratingCriteriaRepo.GetByIdAsync(detail.RatingCriteriaId);
                if (criteria == null)
                    throw new KeyNotFoundException($"Rating criteria with ID {detail.RatingCriteriaId} not found.");
            }

            float avg = (float)model.Details.Average(x => x.Score);

            var workerRating = new WorkerRating
            {
                JobApplicationId = model.JobApplicationId,
                WorkerId = model.WorkerId,
                RatingValue = avg,
                Comment = model.Comment,
                RatedAt = DateTime.UtcNow
            };

            await _unitOfWork.GetRepository<WorkerRating>().AddAsync(workerRating);
            await _unitOfWork.SaveChangesAsync();

            // Add rating criteria separately
            var workerRatingCriteriaRepo = _unitOfWork.GetRepository<WorkerRatingCriteria>();
            foreach (var detail in model.Details)
            {
                var criteriaDetail = new WorkerRatingCriteria
                {
                    WorkerRatingId = workerRating.Id,
                    RatingCriteriaId = detail.RatingCriteriaId,
                    Score = detail.Score
                };
                await workerRatingCriteriaRepo.AddAsync(criteriaDetail);
            }
            await _unitOfWork.SaveChangesAsync();

            // Cập nhật lại Rating và TotalRatings cho Worker
            var worker = await workerRepository.GetByIdAsync(model.WorkerId);
            if (worker != null)
            {
                var ratings = await _unitOfWork.GetRepository<WorkerRating>()
                    .GetAllAsync(r => r.WorkerId == model.WorkerId && !r.IsDeleted);

                worker.TotalRatings = ratings.Count();
                worker.Rating = worker.TotalRatings > 0 ? (float)ratings.Average(r => r.RatingValue) : 0;

                await workerRepository.Update(worker);
                await _unitOfWork.SaveChangesAsync();
            }
        }
    }
}
