using AutoMapper;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<User> _userManager;

        public WorkerService(IUnitOfWork unitOfWork, IMapper mapper, UserManager<User> userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<WorkerModel> GetProfileAsync(string userId)
        {
            var worker = await _unitOfWork.GetRepository<Worker>()
        .GetSingleByConditionAsynce(w => w.UserId == userId, w => w.User);
            if (worker == null)
                throw new KeyNotFoundException("Worker profile not found.");

            return _mapper.Map<WorkerModel>(worker);
        }

        public async Task<WorkerModel> CreateProfileAsync(string userId, WorkerEditModel request)
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
                // Xử lý SocialLinks (nếu có)
                if (request.SocialLinks != null && request.SocialLinks.Count > 0)
                {
                    var socialRepo = _unitOfWork.GetRepository<SocialLink>();
                    foreach (var link in request.SocialLinks)
                    {
                        var entity = _mapper.Map<SocialLink>(link);
                        entity.UserId = userId;
                        await socialRepo.AddAsync(entity);
                    }
                    await _unitOfWork.SaveChangesAsync();
                }
                return await GetProfileAsync(userId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<WorkerModel> UpdateProfileAsync(string userId, WorkerEditModel model)
        {
            var workerRepo = _unitOfWork.GetRepository<Worker>();

            // Lấy worker và user
            var worker = await workerRepo.GetSingleByConditionAsynce(w => w.UserId == userId, w => w.User);
            if (worker == null) throw new KeyNotFoundException("Worker profile not found.");

            // Map các trường Worker
            _mapper.Map(model, worker);

            // Map các trường User (Identity)
            if (worker.User != null)
            {
                _mapper.Map(model, worker.User);
                worker.User.UpdatedAt = DateTime.UtcNow;
                var result = await _userManager.UpdateAsync(worker.User);
                if (!result.Succeeded)
                    throw new Exception(string.Join("; ", result.Errors.Select(e => e.Description)));
            }

            worker.UpdatedAt = DateTime.UtcNow;
            await workerRepo.Update(worker);
            // Xử lý SocialLinks
            var socialRepo = _unitOfWork.GetRepository<SocialLink>();
            var oldLinks = await socialRepo.GetAllAsync(sl => sl.UserId == userId && !sl.IsDeleted);

            // SoftDelete các link cũ
            foreach (var oldLink in oldLinks)
            {
                await socialRepo.SoftDelete(oldLink);
            }

            // Insert lại các link mới
            if (model.SocialLinks != null && model.SocialLinks.Count > 0)
            {
                foreach (var link in model.SocialLinks)
                {
                    var entity = _mapper.Map<SocialLink>(link);
                    entity.UserId = userId;
                    await socialRepo.AddAsync(entity);
                }
            }

            await _unitOfWork.SaveChangesAsync();

            return await GetProfileAsync(userId);
        }
    }
}
