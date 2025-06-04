using AutoMapper;
using Repository.Entities;
using Repository.Interfaces;
using Service.Interfaces;
using Service.Models.CityModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace Service.Services
{
    public class CityService : ICityService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepo<City> _cityRepo;
        private readonly IMapper _mapper;

        public CityService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _cityRepo = _unitOfWork.GetRepository<City>();
            _mapper = mapper;
        }

        // Lấy tất cả bản ghi, kể cả đã xóa mềm
        public async Task<List<CityModel>> GetAllAsync()
        {
            var cities = await _cityRepo.GetAllAsync();
            return _mapper.Map<List<CityModel>>(cities);
        }

        // Lấy tất cả bản ghi chưa xóa mềm (IsDeleted == false)
        public async Task<List<CityModel>> GetAllActiveAsync()
        {
            var cities = await _cityRepo.GetAllAsync(c => !c.IsDeleted);
            return _mapper.Map<List<CityModel>>(cities);
        }

        // Lấy theo ID
        public async Task<CityModel> GetByIdAsync(int id)
        {
            var city = await _cityRepo.GetByIdAsync(id);
            if (city == null)
                throw new KeyNotFoundException("City not found.");
            return _mapper.Map<CityModel>(city);
        }

        // Tạo mới
        public async Task<CityModel> CreateAsync(CreateCityModel dto)
        {
            try
            {
                var city = _mapper.Map<City>(dto);
                await _cityRepo.AddAsync(city);
                await _unitOfWork.SaveChangesAsync();
                return _mapper.Map<CityModel>(city);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to create city: {ex.Message}");
            }
        }

        // Cập nhật
        public async Task<CityModel> UpdateAsync(int id, UpdateCityModel dto)
        {
            var city = await _cityRepo.GetByIdAsync(id);
            if (city == null)
                throw new KeyNotFoundException("City not found.");

            try
            {
                _mapper.Map(dto, city);
                await _cityRepo.Update(city);
                await _unitOfWork.SaveChangesAsync();
                return _mapper.Map<CityModel>(city);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to update city: {ex.Message}");
            }
        }

        // Xóa mềm (đánh dấu IsDeleted = true)
        public async Task<bool> DeleteAsync(int id)
        {
            var city = await _cityRepo.GetByIdAsync(id);
            if (city == null)
                throw new KeyNotFoundException("City not found.");

            try
            {
                await _cityRepo.SoftDelete(city);
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to soft delete city: {ex.Message}");
            }
        }

        // Phục hồi bản ghi xóa mềm (IsDeleted = false)
        public async Task<bool> RestoreAsync(int id)
        {
            var city = await _cityRepo.GetByIdAsync(id);
            if (city == null)
                throw new KeyNotFoundException("City not found.");

            try
            {
                city.IsDeleted = false;
                await _cityRepo.Update(city);
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to restore city: {ex.Message}");
            }
        }

        // Xóa cứng (hard delete) có kiểm tra ràng buộc FK với JobPostings
        public async Task<bool> HardDeleteAsync(int id)
        {
            // Lấy city kèm JobPostings để kiểm tra FK
            var city = await _cityRepo.GetByIdAsync(id, c => c.JobPostings);
            if (city == null)
                throw new KeyNotFoundException("City not found.");

            // Nếu có JobPosting liên quan chưa xóa mềm, không cho xóa cứng
            if (city.JobPostings != null && city.JobPostings.Any(jp => !jp.IsDeleted))
                throw new InvalidOperationException("Cannot hard delete city because it has related active job postings.");

            try
            {
                await _cityRepo.HardDelete(c => c.Id == id);
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to hard delete city: {ex.Message}");
            }
        }
    }
}
