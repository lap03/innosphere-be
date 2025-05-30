using AutoMapper;
using Repository.Entities;
using Repository.Interfaces;
using Service.Interfaces;
using Service.Models.PaymentModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PaymentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<PaymentModel>> GetAllAsync()
        {
            var payments = await _unitOfWork.GetRepository<Payment>().GetAllAsync();
            return _mapper.Map<List<PaymentModel>>(payments);
        }

        public async Task<PaymentModel> GetByIdAsync(int id)
        {
            var payment = await _unitOfWork.GetRepository<Payment>().GetByIdAsync(id,
                p => p.Employer, p => p.Subscription, p => p.PaymentType, p => p.Advertisement);

            if (payment == null)
                throw new KeyNotFoundException("Payment not found."); // Middleware sẽ xử lý lỗi 404

            return _mapper.Map<PaymentModel>(payment);
        }

        public async Task<PaymentModel> CreateAsync(CreatePaymentModel dto)
        {
            try
            {
                var payment = _mapper.Map<Payment>(dto);
                await _unitOfWork.GetRepository<Payment>().AddAsync(payment);
                await _unitOfWork.SaveChangesAsync();
                return _mapper.Map<PaymentModel>(payment);
            }
            catch (Exception ex)
            {
                // Middleware sẽ xử lý lỗi và phản hồi JSON đẹp
                throw new Exception($"Failed to create payment: {ex.Message}");
            }
        }

        public async Task<PaymentModel> UpdateAsync(int id, UpdatePaymentModel dto)
        {
            var repo = _unitOfWork.GetRepository<Payment>();
            var payment = await repo.GetByIdAsync(id);
            if (payment == null)
                throw new KeyNotFoundException("Payment not found."); // Middleware xử lý lỗi 404

            try
            {
                _mapper.Map(dto, payment);
                await repo.Update(payment);
                await _unitOfWork.SaveChangesAsync();
                return _mapper.Map<PaymentModel>(payment);
            }
            catch (Exception ex)
            {
                // Middleware sẽ xử lý lỗi cập nhật
                throw new Exception($"Failed to update payment: {ex.Message}");
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var repo = _unitOfWork.GetRepository<Payment>();
            var payment = await repo.GetByIdAsync(id);
            if (payment == null)
                throw new KeyNotFoundException("Payment not found."); // Middleware xử lý lỗi

            try
            {
                await repo.SoftDelete(payment);
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                // Middleware sẽ trả lỗi dạng JSON
                throw new Exception($"Failed to delete payment: {ex.Message}");
            }
        }
    }
}
