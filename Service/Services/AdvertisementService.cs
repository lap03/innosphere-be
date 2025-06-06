using AutoMapper;
using Repository.Entities;
using Repository.Interfaces;
using Service.Models.AdvertisementModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class AdvertisementService : IAdvertisementService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGenericRepo<Advertisement> _adRepo;
    private readonly IMapper _mapper;

    public AdvertisementService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _adRepo = _unitOfWork.GetRepository<Advertisement>();
        _mapper = mapper;
    }

    // Kiểm tra và cập nhật trạng thái hết hạn hoặc đạt max impression
    private async Task CheckAndUpdateStatusAsync(Advertisement entity)
    {
        var now = DateTime.UtcNow;

        bool expiredByDate = entity.EndDate < now;
        bool expiredByImpressions = entity.MaxImpressions.HasValue && entity.CurrentImpressions >= entity.MaxImpressions.Value;

        if ((expiredByDate || expiredByImpressions) && entity.AdStatus != "EXPIRED")
        {
            entity.AdStatus = "EXPIRED";
            await _adRepo.Update(entity);
            await _unitOfWork.SaveChangesAsync();
        }
    }

    public async Task<List<AdvertisementModel>> GetAllAsync()
    {
        var list = await _adRepo.GetAllAsync();
        foreach (var entity in list)
        {
            await CheckAndUpdateStatusAsync(entity);
        }
        list = await _adRepo.GetAllAsync();
        return _mapper.Map<List<AdvertisementModel>>(list);
    }

    public async Task<List<AdvertisementModel>> GetAllActiveAsync()
    {
        var now = DateTime.UtcNow;
        var list = await _adRepo.GetAllAsync(ad =>
            ad.AdStatus == "ACTIVE" &&
            ad.StartDate <= now &&
            ad.EndDate >= now);

        foreach (var entity in list)
        {
            await CheckAndUpdateStatusAsync(entity);
        }

        list = await _adRepo.GetAllAsync(ad =>
            ad.AdStatus == "ACTIVE" &&
            ad.StartDate <= now &&
            ad.EndDate >= now);

        return _mapper.Map<List<AdvertisementModel>>(list);
    }

    public async Task<AdvertisementModel> GetByIdAsync(int id)
    {
        var entity = await _adRepo.GetByIdAsync(id);
        if (entity == null) throw new KeyNotFoundException("Advertisement not found.");

        await CheckAndUpdateStatusAsync(entity);
        return _mapper.Map<AdvertisementModel>(entity);
    }

    public async Task<AdvertisementModel> CreateAsync(CreateAdvertisementModel dto)
    {
        if (dto.StartDate >= dto.EndDate)
            throw new ArgumentException("StartDate must be before EndDate.");

        var entity = _mapper.Map<Advertisement>(dto);
        entity.AdStatus = "PENDING"; // Mặc định mới tạo là PENDING
        entity.CurrentImpressions = 0;

        await _adRepo.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<AdvertisementModel>(entity);
    }

    public async Task<AdvertisementModel> UpdateAsync(int id, UpdateAdvertisementModel dto)
    {
        var entity = await _adRepo.GetByIdAsync(id);
        if (entity == null) throw new KeyNotFoundException("Advertisement not found.");

        if (dto.StartDate >= dto.EndDate)
            throw new ArgumentException("StartDate must be before EndDate.");

        _mapper.Map(dto, entity);
        await _adRepo.Update(entity);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<AdvertisementModel>(entity);
    }

    public async Task<bool> SoftDeleteAsync(int id)
    {
        var entity = await _adRepo.GetByIdAsync(id);
        if (entity == null) throw new KeyNotFoundException("Advertisement not found.");

        entity.AdStatus = "INACTIVE";
        await _adRepo.Update(entity);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    public async Task<bool> RestoreAsync(int id)
    {
        var entity = await _adRepo.GetByIdAsync(id);
        if (entity == null) throw new KeyNotFoundException("Advertisement not found.");

        entity.AdStatus = "ACTIVE";
        await _adRepo.Update(entity);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    public async Task<bool> HardDeleteAsync(int id)
    {
        var entity = await _adRepo.GetByIdAsync(id);
        if (entity == null) throw new KeyNotFoundException("Advertisement not found.");

        // TODO: Kiểm tra ràng buộc FK nếu cần, vd: payment, click logs, etc.

        await _adRepo.HardDelete(ad => ad.Id == id);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    // Tăng lượt hiển thị quảng cáo
    public async Task<bool> IncrementImpressionAsync(int id)
    {
        var entity = await _adRepo.GetByIdAsync(id);
        if (entity == null) throw new KeyNotFoundException("Advertisement not found.");

        entity.CurrentImpressions++;
        await _adRepo.Update(entity);
        await _unitOfWork.SaveChangesAsync();

        await CheckAndUpdateStatusAsync(entity);
        return true;
    }
}
