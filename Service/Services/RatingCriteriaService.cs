using AutoMapper;
using Repository.Entities;
using Repository.Interfaces;
using Service.Models.RatingCriteriaModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class RatingCriteriaService : IRatingCriteriaService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RatingCriteriaService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<RatingCriteriaModel>> GetAllAsync()
        {
            var list = await _unitOfWork.GetRepository<RatingCriteria>().GetAllAsync(x => !x.IsDeleted);
            return _mapper.Map<IEnumerable<RatingCriteriaModel>>(list);
        }

        public async Task<IEnumerable<RatingCriteriaModel>> GetByTypeAsync(string criteriaType)
        {
            var list = await _unitOfWork.GetRepository<RatingCriteria>().GetAllAsync(
                x => !x.IsDeleted && x.CriteriaType == criteriaType
            );
            return _mapper.Map<IEnumerable<RatingCriteriaModel>>(list);
        }

        public async Task<RatingCriteriaModel> GetByIdAsync(int id)
        {
            var entity = await _unitOfWork.GetRepository<RatingCriteria>().GetByIdAsync(id);
            if (entity == null || entity.IsDeleted) throw new KeyNotFoundException("RatingCriteria not found");
            return _mapper.Map<RatingCriteriaModel>(entity);
        }

        public async Task<RatingCriteriaModel> AddAsync(CreateRatingCriteriaModel model)
        {
            var entity = _mapper.Map<RatingCriteria>(model);

            try
            {
                await _unitOfWork.GetRepository<RatingCriteria>().AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();
                return _mapper.Map<RatingCriteriaModel>(entity);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<RatingCriteriaModel> UpdateAsync(int id, CreateRatingCriteriaModel model)
        {
            var entity = await _unitOfWork.GetRepository<RatingCriteria>().GetByIdAsync(id);
            if (entity == null || entity.IsDeleted) throw new KeyNotFoundException("RatingCriteria not found");
            _mapper.Map(model, entity);

            try
            {
                entity.UpdatedAt = DateTime.UtcNow;
                await _unitOfWork.GetRepository<RatingCriteria>().Update(entity);
                await _unitOfWork.SaveChangesAsync();
                return _mapper.Map<RatingCriteriaModel>(entity);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _unitOfWork.GetRepository<RatingCriteria>().GetByIdAsync(id);
            if (entity == null || entity.IsDeleted) throw new KeyNotFoundException("RatingCriteria not found");
            entity.IsDeleted = true;
            entity.DeletedAt = DateTime.UtcNow;

            try
            {
                await _unitOfWork.GetRepository<RatingCriteria>().Update(entity);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task RestoreAsync(int id)
        {
            var entity = await _unitOfWork.GetRepository<RatingCriteria>().GetByIdAsync(id);
            if (entity == null) throw new KeyNotFoundException("RatingCriteria not found");
            if (!entity.IsDeleted) throw new InvalidOperationException("RatingCriteria is not deleted.");

            entity.IsDeleted = false;
            entity.DeletedAt = null;
            entity.UpdatedAt = DateTime.UtcNow;

            try
            {
                await _unitOfWork.GetRepository<RatingCriteria>().Update(entity);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
