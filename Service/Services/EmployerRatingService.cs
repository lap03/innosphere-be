using AutoMapper;
using Repository.Entities;
using Repository.Interfaces;
using Service.Interfaces;
using Service.Models.EmployerRatingCriteriaModels;
using Service.Models.EmployerRatingModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class EmployerRatingService : IEmployerRatingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EmployerRatingService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<EmployerRatingModel>> GetAllRatingsByEmployerAsync(int employerId)
        {
            var ratings = await _unitOfWork.GetRepository<EmployerRating>()
                .GetAllAsync(r => r.EmployerId == employerId && !r.IsDeleted);

            return _mapper.Map<IEnumerable<EmployerRatingModel>>(ratings);
        }

        public async Task<IEnumerable<EmployerRatingCriteriaModel>> GetRatingDetailsAsync(int employerRatingId)
        {
            var details = await _unitOfWork.GetRepository<EmployerRatingCriteria>()
                .GetAllAsync(d => d.EmployerRatingId == employerRatingId && !d.IsDeleted);

            return _mapper.Map<IEnumerable<EmployerRatingCriteriaModel>>(details);
        }

        public async Task CreateEmployerRatingAsync(CreateEmployerRatingModel model)
        {
            // Kiểm tra JobApplication phải ACCEPTED
            var jobAppRepo = _unitOfWork.GetRepository<JobApplication>();
            var jobApplication = await jobAppRepo.GetByIdAsync(model.JobApplicationId);
            if (jobApplication == null)
                throw new KeyNotFoundException("Job application not found.");
            if (!string.Equals(jobApplication.Status, "ACCEPTED", StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("Job application must be ACCEPTED to rate.");

            // Kiểm tra JobPosting phải COMPLETED hoặc CLOSED
            var jobPosting = jobApplication.JobPosting ?? await _unitOfWork.GetRepository<JobPosting>().GetByIdAsync(jobApplication.JobPostingId);
            if (jobPosting == null)
                throw new KeyNotFoundException("Job posting not found.");
            if (!string.Equals(jobPosting.Status, "COMPLETED", StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(jobPosting.Status, "CLOSED", StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("Job posting must be COMPLETED or CLOSED to rate.");

            if (model.Details == null || !model.Details.Any())
                throw new ArgumentException("Details are required.");

            float avg = (float)model.Details.Average(x => x.Score);

            var employerRating = new EmployerRating
            {
                JobApplicationId = model.JobApplicationId,
                EmployerId = model.EmployerId,
                RatingValue = avg,
                Comment = model.Comment,
                RatedAt = DateTime.UtcNow,
                Details = model.Details.Select(d => new EmployerRatingCriteria
                {
                    RatingCriteriaId = d.RatingCriteriaId,
                    Score = d.Score
                }).ToList()
            };

            await _unitOfWork.GetRepository<EmployerRating>().AddAsync(employerRating);
            await _unitOfWork.SaveChangesAsync();

            // Cập nhật lại Rating và TotalRatings cho Employer (nếu có)
            var employerRepo = _unitOfWork.GetRepository<Employer>();
            var employer = await employerRepo.GetByIdAsync(model.EmployerId);
            if (employer != null)
            {
                var ratings = await _unitOfWork.GetRepository<EmployerRating>()
                    .GetAllAsync(r => r.EmployerId == model.EmployerId && !r.IsDeleted);

                employer.TotalRatings = ratings.Count();
                employer.Rating = employer.TotalRatings > 0 ? (float)ratings.Average(r => r.RatingValue) : 0;

                await employerRepo.Update(employer);
                await _unitOfWork.SaveChangesAsync();
            }
        }
    }
}
