using AutoMapper;
using Repository.Entities;
using Repository.Interfaces;
using Service.Interfaces;
using Service.Models.ResumeModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class ResumeService : IResumeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ResumeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ResumeModel>> GetResumesByWorkerAsync(int workerId)
        {
            var resumes = await _unitOfWork.GetRepository<Resume>().GetAllAsync(r => r.WorkerId == workerId && !r.IsDeleted);
            return _mapper.Map<IEnumerable<ResumeModel>>(resumes);
        }

        public async Task<ResumeModel> GetResumeAsync(int id)
        {
            var resume = await _unitOfWork.GetRepository<Resume>().GetByIdAsync(id);
            if (resume == null) throw new KeyNotFoundException("Resume not found");
            if (resume.IsDeleted) throw new KeyNotFoundException("Resume has been deleted");


            return _mapper.Map<ResumeModel>(resume);
        }

        public async Task<ResumeModel> AddResumeAsync(CreateResumeModel model)
        {
            var resume = _mapper.Map<Resume>(model);

            try
            {
                await _unitOfWork.GetRepository<Resume>().AddAsync(resume);
                await _unitOfWork.SaveChangesAsync();

                return _mapper.Map<ResumeModel>(resume);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<ResumeModel> UpdateResumeAsync(int id, CreateResumeModel model)
        {
            var resume = await _unitOfWork.GetRepository<Resume>().GetByIdAsync(id);
            if (resume == null) throw new KeyNotFoundException("Resume not found");
            if (resume.WorkerId != model.WorkerId) throw new UnauthorizedAccessException("You do not have permission to update this resume.");

            try
            {
                _mapper.Map(model, resume);
                resume.UpdatedAt = DateTime.UtcNow;

                await _unitOfWork.GetRepository<Resume>().Update(resume);
                return _mapper.Map<ResumeModel>(resume);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task DeleteResumeAsync(int id)
        {
            var resume = await _unitOfWork.GetRepository<Resume>().GetByIdAsync(id);
            if (resume == null) throw new KeyNotFoundException("Resume not found");
            if (resume.IsDeleted) throw new KeyNotFoundException("Resume has already been deleted");

            await _unitOfWork.GetRepository<Resume>().SoftDelete(resume);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task RestoreResumeAsync(int id)
        {
            var resume = await _unitOfWork.GetRepository<Resume>().GetByIdAsync(id);
            if (resume == null) throw new KeyNotFoundException("Resume not found");
            if (!resume.IsDeleted) throw new InvalidOperationException("Resume is not deleted and cannot be restored.");

            resume.IsDeleted = false;

            await _unitOfWork.GetRepository<Resume>().Update(resume);
        }
    }
}
