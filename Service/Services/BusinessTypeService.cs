using AutoMapper;
using Repository.Entities;
using Repository.Interfaces;
using Service.Interfaces;
using Service.Models.BusinessTypeModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class BusinessTypeService : IBusinessTypeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BusinessTypeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<BusinessTypeModel>> GetAllAsync()
        {
            var list = await _unitOfWork.GetRepository<BusinessType>()
                .GetAllAsync(bt => !bt.IsDeleted);

            return _mapper.Map<List<BusinessTypeModel>>(list);
        }

        public async Task<BusinessTypeModel> GetByIdAsync(int id)
        {
            var entity = await _unitOfWork.GetRepository<BusinessType>().GetByIdAsync(id);
            if (entity == null || entity.IsDeleted)
                throw new KeyNotFoundException("Business type not found.");

            return _mapper.Map<BusinessTypeModel>(entity);
        }

        public async Task<BusinessTypeModel> CreateAsync(CreateBusinessTypeModel model)
        {
            var entity = _mapper.Map<BusinessType>(model);
            await _unitOfWork.GetRepository<BusinessType>().AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<BusinessTypeModel>(entity);
        }

        public async Task<BusinessTypeModel> UpdateAsync(int id, UpdateBusinessTypeModel model)
        {
            var repo = _unitOfWork.GetRepository<BusinessType>();
            var entity = await repo.GetByIdAsync(id);

            if (entity == null || entity.IsDeleted)
                throw new KeyNotFoundException("Business type not found.");

            _mapper.Map(model, entity);
            await repo.Update(entity);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<BusinessTypeModel>(entity);
        }

        public async Task<bool> SoftDeleteAsync(int id)
        {
            var repo = _unitOfWork.GetRepository<BusinessType>();
            var entity = await repo.GetByIdAsync(id);

            if (entity == null || entity.IsDeleted)
                throw new KeyNotFoundException("Business type not found.");

            await repo.SoftDelete(entity);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
