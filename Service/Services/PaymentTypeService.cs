using AutoMapper;
using Repository.Entities;
using Repository.Interfaces;
using Service.Interfaces;
using Service.Models.PaymentTypeModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Services
{
    public class PaymentTypeService : IPaymentTypeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepo<PaymentType> _paymentTypeRepo;
        private readonly IMapper _mapper;

        public PaymentTypeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _paymentTypeRepo = _unitOfWork.GetRepository<PaymentType>();
            _mapper = mapper;
        }

        public async Task<List<PaymentTypeModel>> GetAllAsync()
        {
            var list = await _paymentTypeRepo.GetAllAsync();
            return _mapper.Map<List<PaymentTypeModel>>(list);
        }

        public async Task<List<PaymentTypeModel>> GetAllActiveAsync()
        {
            var list = await _paymentTypeRepo.GetAllAsync(pt => !pt.IsDeleted);
            return _mapper.Map<List<PaymentTypeModel>>(list);
        }

        public async Task<PaymentTypeModel> GetByIdAsync(int id)
        {
            var entity = await _paymentTypeRepo.GetByIdAsync(id);
            if (entity == null) throw new KeyNotFoundException("Payment type not found.");
            return _mapper.Map<PaymentTypeModel>(entity);
        }

        public async Task<PaymentTypeModel> CreateAsync(CreatePaymentTypeModel dto)
        {
            var entity = _mapper.Map<PaymentType>(dto);
            await _paymentTypeRepo.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<PaymentTypeModel>(entity);
        }

        public async Task<PaymentTypeModel> UpdateAsync(int id, UpdatePaymentTypeModel dto)
        {
            var entity = await _paymentTypeRepo.GetByIdAsync(id);
            if (entity == null) throw new KeyNotFoundException("Payment type not found.");

            _mapper.Map(dto, entity);
            await _paymentTypeRepo.Update(entity);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<PaymentTypeModel>(entity);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _paymentTypeRepo.GetByIdAsync(id);
            if (entity == null) throw new KeyNotFoundException("Payment type not found.");

            try
            {
                await _paymentTypeRepo.HardDelete(pt => pt.Id == id);
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to delete payment type: {ex.Message}");
            }
        }

        public async Task<bool> RestoreAsync(int id)
        {
            var entity = await _paymentTypeRepo.GetByIdAsync(id);
            if (entity == null) throw new KeyNotFoundException("Payment type not found.");

            entity.IsDeleted = false;
            await _paymentTypeRepo.Update(entity);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> HardDeleteAsync(int id)
        {
            var entity = await _paymentTypeRepo.GetByIdAsync(id);
            if (entity == null) throw new KeyNotFoundException("Payment type not found.");

            try
            {
                await _paymentTypeRepo.HardDelete(pt => pt.Id == id);
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to hard delete payment type: {ex.Message}");
            }
        }
    }
}
