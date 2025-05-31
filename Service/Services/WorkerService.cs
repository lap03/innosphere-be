using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Repository.Entities;
using Repository.Interfaces;
using Service.Interfaces;
using Service.Models.WorkerModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class WorkerService : IWorkerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public WorkerService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Worker> GetProfileAsync(string userId)
        {
            var worker = await _unitOfWork.GetRepository<Worker>().GetSingleByConditionAsynce(u => u.UserId == userId);
            if (worker == null)
                throw new KeyNotFoundException("Worker profile not found.");
            return worker;
        }

        public async Task<Worker> CreateProfileAsync(string userId, WorkerEditModel request)
        {
            var repo = _unitOfWork.GetRepository<Worker>();
            if (await repo.GetSingleByConditionAsynce(w => w.UserId == userId) != null)
                throw new InvalidOperationException("Worker profile already exists.");

            try
            {
                var worker = _mapper.Map<Worker>(request);
                worker.UserId = userId;
                worker.CreatedAt = DateTime.UtcNow; // Cập nhật thời gian tạo
                worker.VerificationStatus = "PENDING";
                await repo.AddAsync(worker);
                await _unitOfWork.SaveChangesAsync();
                return worker;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Worker> UpdateProfileAsync(string userId, WorkerEditModel request)
        {
            var repo = _unitOfWork.GetRepository<Worker>();
            var worker = await repo.GetSingleByConditionAsynce(w => w.UserId == userId);
            if (worker == null)
                throw new KeyNotFoundException("Worker profile not found.");
            try
            {
                _mapper.Map(request, worker);
                worker.UpdatedAt = DateTime.UtcNow; // Cập nhật thời gian sửa đổi

                await repo.Update(worker);
                await _unitOfWork.SaveChangesAsync();
                return worker;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
