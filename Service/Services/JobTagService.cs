using AutoMapper;
using Repository.Entities;
using Repository.Interfaces;
using Service.Interfaces;
using Service.Models.JobTagModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Services
{
    public class JobTagService : IJobTagService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepo<JobTag> _jobTagRepo;
        private readonly IMapper _mapper;

        public JobTagService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _jobTagRepo = _unitOfWork.GetRepository<JobTag>();
            _mapper = mapper;
        }

        public async Task<List<JobTagModel>> GetAllAsync()
        {
            var list = await _jobTagRepo.GetAllAsync();
            return _mapper.Map<List<JobTagModel>>(list);
        }

        public async Task<List<JobTagModel>> GetAllActiveAsync()
        {
            var list = await _jobTagRepo.GetAllAsync(jt => !jt.IsDeleted);
            return _mapper.Map<List<JobTagModel>>(list);
        }

        public async Task<JobTagModel> GetByIdAsync(int id)
        {
            var entity = await _jobTagRepo.GetByIdAsync(id);
            if (entity == null) throw new KeyNotFoundException("Job tag not found.");
            return _mapper.Map<JobTagModel>(entity);
        }

        public async Task<JobTagModel> CreateAsync(CreateJobTagModel dto)
        {
            try
            {
                var entity = _mapper.Map<JobTag>(dto);
                await _jobTagRepo.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();
                return _mapper.Map<JobTagModel>(entity);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to create job tag: {ex.Message}");
            }
        }

        public async Task<JobTagModel> UpdateAsync(int id, UpdateJobTagModel dto)
        {
            var entity = await _jobTagRepo.GetByIdAsync(id);
            if (entity == null) throw new KeyNotFoundException("Job tag not found.");

            try
            {
                _mapper.Map(dto, entity);
                await _jobTagRepo.Update(entity);
                await _unitOfWork.SaveChangesAsync();
                return _mapper.Map<JobTagModel>(entity);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to update job tag: {ex.Message}");
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _jobTagRepo.GetByIdAsync(id);
            if (entity == null) throw new KeyNotFoundException("Job tag not found.");

            try
            {
                await _jobTagRepo.SoftDelete(entity);
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to soft delete job tag: {ex.Message}");
            }
        }

        public async Task<bool> RestoreAsync(int id)
        {
            var entity = await _jobTagRepo.GetByIdAsync(id);
            if (entity == null) throw new KeyNotFoundException("Job tag not found.");

            try
            {
                entity.IsDeleted = false;
                await _jobTagRepo.Update(entity);
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to restore job tag: {ex.Message}");
            }
        }

        public async Task<bool> HardDeleteAsync(int id)
        {
            // Lấy entity kèm JobPostingTags (FK)
            var entity = await _jobTagRepo.GetByIdAsync(id, jt => jt.JobPostingTags);
            if (entity == null) throw new KeyNotFoundException("Job tag not found.");

            // Kiểm tra có bản ghi con chưa xóa mềm không
            if (entity.JobPostingTags != null && entity.JobPostingTags.Any(jpt => !jpt.IsDeleted))
                throw new InvalidOperationException("Cannot hard delete job tag because it has related active job posting tags.");

            try
            {
                await _jobTagRepo.HardDelete(jt => jt.Id == id);
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to hard delete job tag: {ex.Message}");
            }
        }
    }
}
