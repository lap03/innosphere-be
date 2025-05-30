using AutoMapper;

using Repository.Entities;
using Repository.Interfaces;
using Service.Interfaces;
using Service.Models.CityModels;

namespace innosphere_be.Services.Implementations
{
    public class CityService : ICityService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CityService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<CityModel>> GetAllAsync()
        {
            var cities = await _unitOfWork.GetRepository<City>().GetAllAsync();
            return _mapper.Map<List<CityModel>>(cities);
        }

        public async Task<CityModel> GetByIdAsync(int id)
        {
            var city = await _unitOfWork.GetRepository<City>().GetByIdAsync(id);
            if (city == null)
                throw new KeyNotFoundException("City not found."); // Lỗi sẽ được middleware bắt và trả về 404

            return _mapper.Map<CityModel>(city);
        }

        public async Task<CityModel> CreateAsync(CreateCityModel dto)
        {
            try
            {
                var city = _mapper.Map<City>(dto);
                await _unitOfWork.GetRepository<City>().AddAsync(city);
                await _unitOfWork.SaveChangesAsync();
                return _mapper.Map<CityModel>(city);
            }
            catch (Exception ex)
            {
                // Middleware sẽ bắt exception này và trả response lỗi 500 (hoặc tuỳ loại exception)
                throw new Exception($"Failed to create city: {ex.Message}");
            }
        }

        public async Task<CityModel> UpdateAsync(int id, UpdateCityModel dto)
        {
            var cityRepo = _unitOfWork.GetRepository<City>();
            var city = await cityRepo.GetByIdAsync(id);
            if (city == null)
                throw new KeyNotFoundException("City not found."); // Middleware sẽ xử lý và trả về JSON lỗi

            try
            {
                _mapper.Map(dto, city);
                await cityRepo.Update(city);
                await _unitOfWork.SaveChangesAsync();
                return _mapper.Map<CityModel>(city);
            }
            catch (Exception ex)
            {
                // Nếu có lỗi DB hoặc logic, middleware sẽ nhận và định dạng phản hồi lỗi
                throw new Exception($"Failed to update city: {ex.Message}");
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var cityRepo = _unitOfWork.GetRepository<City>();
            var city = await cityRepo.GetByIdAsync(id);
            if (city == null)
                throw new KeyNotFoundException("City not found."); // Middleware sẽ trả về lỗi 404

            try
            {
                await cityRepo.SoftDelete(city);
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                // Middleware sẽ xử lý lỗi này và trả về lỗi định dạng JSON
                throw new Exception($"Failed to delete city: {ex.Message}");
            }
        }
    }
}
