using AutoMapper;
using Repository.Entities;
using Repository.Interfaces;
using Service.Interfaces;
using Service.Models.AdvertisementModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class AdvertisementService : IAdvertisementService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AdvertisementService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    // Kiểm tra và cập nhật trạng thái hết hạn hoặc đạt max impression
    private async Task CheckAndUpdateStatusAsync(Advertisement entity)
    {
        var now = DateTime.UtcNow;

        bool expiredByDate = entity.EndDate < now;
        bool expiredByImpressions = entity.MaxImpressions.HasValue && entity.CurrentImpressions >= entity.MaxImpressions.Value;

        // Nếu hết hạn theo thời gian hoặc lượt hiển thị, cập nhật trạng thái quảng cáo thành EXPIRED
        if ((expiredByDate || expiredByImpressions) && entity.AdStatus != "EXPIRED")
        {
            entity.AdStatus = "EXPIRED";
            await _unitOfWork.GetRepository<Advertisement>().Update(entity);
            await _unitOfWork.SaveChangesAsync();
        }
    }

    /// <summary>
    /// Lấy tất cả quảng cáo của một employer, bao gồm kiểm tra trạng thái hết hạn.
    /// </summary>
    public async Task<List<AdvertisementModel>> GetAllByEmployerAsync(int employerId)
    {
        var repo = _unitOfWork.GetRepository<Advertisement>();
        var now = DateTime.UtcNow;

        // Lấy tất cả quảng cáo của employer không bị xóa
        var list = await repo.GetAllAsync(ad =>
            ad.EmployerId == employerId &&
            !ad.IsDeleted);

        // Cập nhật trạng thái hết hạn cho từng quảng cáo
        foreach (var entity in list)
        {
            await CheckAndUpdateStatusAsync(entity);
        }

        // Lấy lại danh sách đã cập nhật trạng thái
        list = await repo.GetAllAsync(ad =>
            ad.EmployerId == employerId &&
            !ad.IsDeleted);

        return _mapper.Map<List<AdvertisementModel>>(list);
    }

    /// <summary>
    /// Lấy tất cả quảng cáo đang active và còn hạn của employer
    /// </summary>
    public async Task<List<AdvertisementModel>> GetAllActiveByEmployerAsync(int employerId)
    {
        var repo = _unitOfWork.GetRepository<Advertisement>();
        var now = DateTime.UtcNow;

        var list = await repo.GetAllAsync(ad =>
            ad.EmployerId == employerId &&
            ad.AdStatus == "ACTIVE" &&
            ad.StartDate <= now &&
            ad.EndDate >= now &&
            !ad.IsDeleted);

        foreach (var entity in list)
        {
            await CheckAndUpdateStatusAsync(entity);
        }

        list = await repo.GetAllAsync(ad =>
            ad.EmployerId == employerId &&
            ad.AdStatus == "ACTIVE" &&
            ad.StartDate <= now &&
            ad.EndDate >= now &&
            !ad.IsDeleted);

        return _mapper.Map<List<AdvertisementModel>>(list);
    }

    /// <summary>
    /// Lấy quảng cáo theo Id và kiểm tra thuộc về employer
    /// </summary>
    public async Task<AdvertisementModel> GetByIdAsync(int id, int employerId)
    {
        var repo = _unitOfWork.GetRepository<Advertisement>();
        var entity = await repo.GetByIdAsync(id);

        if (entity == null)
            throw new KeyNotFoundException("Advertisement not found.");

        if (entity.EmployerId != employerId)
            throw new UnauthorizedAccessException("Access denied.");

        await CheckAndUpdateStatusAsync(entity);

        return _mapper.Map<AdvertisementModel>(entity);
    }

    /// <summary>
    /// Tạo quảng cáo mới cho employer
    /// </summary>
    public async Task<AdvertisementModel> CreateAsync(CreateAdvertisementModel dto)
    {
        if (dto.StartDate >= dto.EndDate)
            throw new ArgumentException("StartDate must be before EndDate.");

        // Gán trạng thái PENDING mặc định
        var entity = _mapper.Map<Advertisement>(dto);
        entity.AdStatus = "PENDING";
        entity.CurrentImpressions = 0;

        var repo = _unitOfWork.GetRepository<Advertisement>();
        await repo.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<AdvertisementModel>(entity);
    }

    /// <summary>
    /// Cập nhật quảng cáo chỉ khi thuộc về employer
    /// </summary>
    public async Task<AdvertisementModel> UpdateAsync(int id, int employerId, UpdateAdvertisementModel dto)
    {
        var repo = _unitOfWork.GetRepository<Advertisement>();
        var entity = await repo.GetByIdAsync(id);

        if (entity == null)
            throw new KeyNotFoundException("Advertisement not found.");

        if (entity.EmployerId != employerId)
            throw new UnauthorizedAccessException("Access denied.");

        if (dto.StartDate >= dto.EndDate)
            throw new ArgumentException("StartDate must be before EndDate.");

        _mapper.Map(dto, entity);
        await repo.Update(entity);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<AdvertisementModel>(entity);
    }

    /// <summary>
    /// Soft delete quảng cáo bằng cách chuyển trạng thái thành INACTIVE (chỉ nếu thuộc employer)
    /// </summary>
    public async Task<bool> SoftDeleteAsync(int id, int employerId)
    {
        var repo = _unitOfWork.GetRepository<Advertisement>();
        var entity = await repo.GetByIdAsync(id);

        if (entity == null)
            throw new KeyNotFoundException("Advertisement not found.");

        if (entity.EmployerId != employerId)
            throw new UnauthorizedAccessException("Access denied.");

        entity.AdStatus = "INACTIVE";
        await repo.Update(entity);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    /// <summary>
    /// Khôi phục quảng cáo (chỉ nếu thuộc employer)
    /// </summary>
    public async Task<bool> RestoreAsync(int id, int employerId)
    {
        var repo = _unitOfWork.GetRepository<Advertisement>();
        var entity = await repo.GetByIdAsync(id);

        if (entity == null)
            throw new KeyNotFoundException("Advertisement not found.");

        if (entity.EmployerId != employerId)
            throw new UnauthorizedAccessException("Access denied.");

        entity.AdStatus = "ACTIVE";
        await repo.Update(entity);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    /// <summary>
    /// Xóa cứng quảng cáo (chỉ nếu thuộc employer)
    /// TODO: Cần kiểm tra ràng buộc FK nếu có
    /// </summary>
    public async Task<bool> HardDeleteAsync(int id, int employerId)
    {
        var repo = _unitOfWork.GetRepository<Advertisement>();
        var entity = await repo.GetByIdAsync(id);

        if (entity == null)
            throw new KeyNotFoundException("Advertisement not found.");

        if (entity.EmployerId != employerId)
            throw new UnauthorizedAccessException("Access denied.");

        // TODO: Kiểm tra ràng buộc FK với các bảng khác nếu có

        await repo.HardDelete(ad => ad.Id == id);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    /// <summary>
    /// Tăng lượt hiển thị quảng cáo, cập nhật trạng thái nếu đạt max
    /// </summary>
    public async Task<bool> IncrementImpressionAsync(int id)
    {
        var repo = _unitOfWork.GetRepository<Advertisement>();
        var entity = await repo.GetByIdAsync(id);

        if (entity == null)
            throw new KeyNotFoundException("Advertisement not found.");

        entity.CurrentImpressions++;
        await repo.Update(entity);
        await _unitOfWork.SaveChangesAsync();

        await CheckAndUpdateStatusAsync(entity);
        return true;
    }

    /// <summary>
    /// Admin: Lấy tất cả quảng cáo từ tất cả user
    /// </summary>
    public async Task<List<AdvertisementModel>> GetAllForAdminAsync()
    {
        var repo = _unitOfWork.GetRepository<Advertisement>();

        // Lấy tất cả quảng cáo không bị xóa cứng, bao gồm thông tin employer và user
        var list = await repo.GetAllAsync(ad => !ad.IsDeleted, ad => ad.Employer, ad => ad.Employer.User);

        // Cập nhật trạng thái hết hạn cho từng quảng cáo
        foreach (var entity in list)
        {
            await CheckAndUpdateStatusAsync(entity);
        }

        // Lấy lại danh sách đã cập nhật trạng thái
        list = await repo.GetAllAsync(ad => !ad.IsDeleted, ad => ad.Employer, ad => ad.Employer.User);

        return _mapper.Map<List<AdvertisementModel>>(list);
    }

    /// <summary>
    /// Admin: Update advertisement status
    /// </summary>
    public async Task<bool> UpdateAdvertisementStatusAsync(int id, string status)
    {
        var repo = _unitOfWork.GetRepository<Advertisement>();
        var entity = await repo.GetByIdAsync(id);

        if (entity == null)
            return false;

        // Only allow valid statuses
        var validStatuses = new[] { "PENDING", "ACTIVE", "INACTIVE", "EXPIRED" };
        if (!validStatuses.Contains(status))
            return false;

        entity.AdStatus = status;
        await repo.Update(entity);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }
}
