using AutoMapper;
using Repository.Entities;
using Repository.Interfaces;
using Service.Interfaces;
using Service.Models.NotificationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepo<Notification> _repo;
        private readonly IMapper _mapper;

        public NotificationService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _repo = _unitOfWork.GetRepository<Notification>();
            _mapper = mapper;
        }

        public async Task<List<NotificationModel>> GetAllAsync()
        {
            var list = await _repo.GetAllAsync();
            return _mapper.Map<List<NotificationModel>>(list);
        }

        public async Task<List<NotificationModel>> GetAllActiveAsync()
        {
            var list = await _repo.GetAllAsync(n => !n.IsDeleted);
            return _mapper.Map<List<NotificationModel>>(list);
        }

        public async Task<NotificationModel> GetByIdAsync(int id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) throw new KeyNotFoundException("Notification not found.");
            return _mapper.Map<NotificationModel>(entity);
        }

        public async Task<NotificationModel> CreateAsync(CreateNotificationModel dto)
        {
            try
            {
                var entity = _mapper.Map<Notification>(dto);
                await _repo.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();
                return _mapper.Map<NotificationModel>(entity);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to create notification: {ex.Message}");
            }
        }

        public async Task<NotificationModel> UpdateAsync(int id, UpdateNotificationModel dto)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) throw new KeyNotFoundException("Notification not found.");

            try
            {
                _mapper.Map(dto, entity);
                await _repo.Update(entity);
                await _unitOfWork.SaveChangesAsync();
                return _mapper.Map<NotificationModel>(entity);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to update notification: {ex.Message}");
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) throw new KeyNotFoundException("Notification not found.");

            try
            {
                await _repo.SoftDelete(entity);
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to soft delete notification: {ex.Message}");
            }
        }

        public async Task<bool> RestoreAsync(int id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) throw new KeyNotFoundException("Notification not found.");

            try
            {
                entity.IsDeleted = false;
                await _repo.Update(entity);
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to restore notification: {ex.Message}");
            }
        }

        public async Task<bool> HardDeleteAsync(int id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) throw new KeyNotFoundException("Notification not found.");

            // TODO: Nếu Notification có FK phụ thuộc, check ở đây trước khi xóa cứng

            try
            {
                await _repo.HardDelete(n => n.Id == id);
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to hard delete notification: {ex.Message}");
            }
        }
    }
}
