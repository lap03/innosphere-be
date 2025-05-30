using AutoMapper;
using Repository.Entities;
using Repository.Interfaces;
using Service.Interfaces;
using Service.Models.JobTagModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class JobTagService : IJobTagService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public JobTagService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<JobTagModel>> GetAllAsync()
        {
            var tags = await _unitOfWork.GetRepository<JobTag>().GetAllAsync();
            return _mapper.Map<List<JobTagModel>>(tags);
        }

        public async Task<JobTagModel> GetByIdAsync(int id)
        {
            var tag = await _unitOfWork.GetRepository<JobTag>().GetByIdAsync(id);
            if (tag == null)
                throw new KeyNotFoundException("Job tag not found."); // Middleware sẽ bắt lỗi 404

            return _mapper.Map<JobTagModel>(tag);
        }

        public async Task<JobTagModel> CreateAsync(CreateJobTagModel dto)
        {
            try
            {
                var tag = _mapper.Map<JobTag>(dto);
                await _unitOfWork.GetRepository<JobTag>().AddAsync(tag);
                await _unitOfWork.SaveChangesAsync();
                return _mapper.Map<JobTagModel>(tag);
            }
            catch (Exception ex)
            {
                // Nếu có lỗi khi thêm, middleware sẽ xử lý lỗi này
                throw new Exception($"Failed to create job tag: {ex.Message}");
            }
        }

        public async Task<JobTagModel> UpdateAsync(int id, UpdateJobTagModel dto)
        {
            var tagRepo = _unitOfWork.GetRepository<JobTag>();
            var tag = await tagRepo.GetByIdAsync(id);
            if (tag == null)
                throw new KeyNotFoundException("Job tag not found."); // Middleware xử lý lỗi không tìm thấy

            try
            {
                _mapper.Map(dto, tag);
                await tagRepo.Update(tag);
                await _unitOfWork.SaveChangesAsync();
                return _mapper.Map<JobTagModel>(tag);
            }
            catch (Exception ex)
            {
                // Middleware sẽ bắt lỗi cập nhật thất bại
                throw new Exception($"Failed to update job tag: {ex.Message}");
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var tagRepo = _unitOfWork.GetRepository<JobTag>();
            var tag = await tagRepo.GetByIdAsync(id);
            if (tag == null)
                throw new KeyNotFoundException("Job tag not found."); // Middleware bắt lỗi 404

            try
            {
                await tagRepo.SoftDelete(tag);
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                // Middleware xử lý lỗi nếu soft delete thất bại
                throw new Exception($"Failed to delete job tag: {ex.Message}");
            }
        }
    }
}
