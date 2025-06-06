using Microsoft.AspNetCore.Identity;
using Repository.Entities;
using Repository.Helpers;
using Repository.Interfaces;
using Service.Interfaces;

namespace Service.Services
{
    public class InitService : IInitService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUnitOfWork _unitOfWork;

        public InitService(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _unitOfWork = unitOfWork;
        }

        // Khởi tạo dữ liệu mặc định cho các bảng như City, JobTag, PaymentType, BusinessType
        public async Task<IdentityResult> SeedCitiesAsync()
        {
            try
            {
                var cities = new List<City>
                {
                    new City {CityName = "Hanoi", Country = "Vietnam", IsDeleted = false, CreatedAt = DateTime.UtcNow },
                    new City {CityName = "Ho Chi Minh City", Country = "Vietnam", IsDeleted = false, CreatedAt = DateTime.UtcNow },
                    new City {CityName = "Da Nang", Country = "Vietnam", IsDeleted = false, CreatedAt = DateTime.UtcNow }
                };
                var repo = _unitOfWork.GetRepository<City>();

                if (await repo.AnyAsync())
                {
                    return IdentityResult.Failed(new IdentityError { Description = "Cities already exist." });
                }

                await repo.AddRangeAsync(cities);
                await _unitOfWork.SaveChangesAsync();

                return IdentityResult.Success;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to init city: {ex.Message}");
            }
        }

        public async Task<IdentityResult> SeedJobTagsAsync()
        {
            try
            {
                var jobTags = new List<JobTag>
                {
                    new JobTag { TagName = "Full-Time", IsDeleted = false, CreatedAt = DateTime.UtcNow },
                    new JobTag { TagName = "Part-Time", IsDeleted = false, CreatedAt = DateTime.UtcNow },
                    new JobTag { TagName = "Internship", IsDeleted = false, CreatedAt = DateTime.UtcNow }
                };
                var repo = _unitOfWork.GetRepository<JobTag>();

                if (await repo.AnyAsync())
                {
                    return IdentityResult.Failed(new IdentityError { Description = "JobTags already exist." });
                }

                await repo.AddRangeAsync(jobTags);
                await _unitOfWork.SaveChangesAsync();

                return IdentityResult.Success;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to init job tags: {ex.Message}");
            }
        }

        public async Task<IdentityResult> SeedPaymentTypesAsync()
        {
            try
            {
                var paymentTypes = new List<PaymentType>
                {
                    new PaymentType { TypeName = "Credit Card", Description = "Payments by credit card", IsDeleted = false, CreatedAt = DateTime.UtcNow },
                    new PaymentType { TypeName = "Paypal", Description = "Payments by Paypal", IsDeleted = false, CreatedAt = DateTime.UtcNow },
                    new PaymentType { TypeName = "Bank Transfer", Description = "Payments by bank transfer", IsDeleted = false, CreatedAt = DateTime.UtcNow }
                };
                var repo = _unitOfWork.GetRepository<PaymentType>();

                if (await repo.AnyAsync())
                {
                    return IdentityResult.Failed(new IdentityError { Description = "PaymentTypes already exist." });
                }

                await repo.AddRangeAsync(paymentTypes);
                await _unitOfWork.SaveChangesAsync();

                return IdentityResult.Success;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to init payment types: {ex.Message}");
            }
        }

        public async Task<IdentityResult> SeedBusinessTypesAsync()
        {
            try
            {
                var businessTypes = new List<BusinessType>
                {
                    new BusinessType { Name = "Doanh nghiệp tư nhân", Description = "Doanh nghiệp do một cá nhân sở hữu và chịu trách nhiệm vô hạn.", IsDeleted = false, CreatedAt = DateTime.UtcNow },
                    new BusinessType { Name = "Hộ kinh doanh cá thể", Description = "Phổ biến ở Việt Nam, thường là các cửa hàng nhỏ lẻ, quán ăn, dịch vụ gia đình.", IsDeleted = false, CreatedAt = DateTime.UtcNow },
                    new BusinessType { Name = "Công ty trách nhiệm hữu hạn (TNHH)", Description = "Bao gồm TNHH một thành viên hoặc nhiều thành viên, phổ biến ở Việt Nam.", IsDeleted = false, CreatedAt = DateTime.UtcNow },
                    new BusinessType { Name = "Công ty cổ phần", Description = "Doanh nghiệp có nhiều cổ đông, phù hợp với các công ty lớn.", IsDeleted = false, CreatedAt = DateTime.UtcNow },
                    new BusinessType { Name = "Hợp tác xã", Description = "Tổ chức kinh tế tập thể, ví dụ: hợp tác xã nông nghiệp, tiêu dùng.", IsDeleted = false, CreatedAt = DateTime.UtcNow },
                    new BusinessType { Name = "Doanh nghiệp nhỏ", Description = "Doanh nghiệp quy mô nhỏ, thường dưới 50 nhân viên.", IsDeleted = false, CreatedAt = DateTime.UtcNow },
                    new BusinessType { Name = "Doanh nghiệp vừa", Description = "Quy mô trung bình, thường từ 50-250 nhân viên.", IsDeleted = false, CreatedAt = DateTime.UtcNow },
                    new BusinessType { Name = "Doanh nghiệp gia đình", Description = "Doanh nghiệp do các thành viên trong gia đình sở hữu và điều hành.", IsDeleted = false, CreatedAt = DateTime.UtcNow },
                    new BusinessType { Name = "Bán lẻ", Description = "Kinh doanh bán hàng trực tiếp đến người tiêu dùng, ví dụ: cửa hàng tạp hóa, siêu thị.", IsDeleted = false, CreatedAt = DateTime.UtcNow },
                    new BusinessType { Name = "Thương mại điện tử", Description = "Kinh doanh trực tuyến, ví dụ: Shopee, Lazada.", IsDeleted = false, CreatedAt = DateTime.UtcNow },
                    new BusinessType { Name = "Sản xuất", Description = "Sản xuất hàng hóa, ví dụ: sản xuất thực phẩm, quần áo.", IsDeleted = false, CreatedAt = DateTime.UtcNow },
                    new BusinessType { Name = "Dịch vụ", Description = "Cung cấp dịch vụ, ví dụ: dịch vụ tài chính, vận tải, giáo dục.", IsDeleted = false, CreatedAt = DateTime.UtcNow },
                    new BusinessType { Name = "Công nghệ thông tin", Description = "Phát triển phần mềm, dịch vụ công nghệ.", IsDeleted = false, CreatedAt = DateTime.UtcNow },
                    new BusinessType { Name = "Nông nghiệp", Description = "Sản xuất nông sản, chăn nuôi, thủy sản.", IsDeleted = false, CreatedAt = DateTime.UtcNow },
                    new BusinessType { Name = "Xây dựng", Description = "Kinh doanh xây dựng nhà cửa, công trình.", IsDeleted = false, CreatedAt = DateTime.UtcNow },
                    new BusinessType { Name = "Bất động sản", Description = "Mua bán, cho thuê bất động sản.", IsDeleted = false, CreatedAt = DateTime.UtcNow },
                    new BusinessType { Name = "Y tế", Description = "Bệnh viện, phòng khám, dịch vụ chăm sóc sức khỏe.", IsDeleted = false, CreatedAt = DateTime.UtcNow },
                    new BusinessType { Name = "Du lịch", Description = "Công ty lữ hành, dịch vụ du lịch.", IsDeleted = false, CreatedAt = DateTime.UtcNow },
                    new BusinessType { Name = "Vận tải", Description = "Dịch vụ logistics, vận chuyển hàng hóa hoặc hành khách.", IsDeleted = false, CreatedAt = DateTime.UtcNow },
                    new BusinessType { Name = "Tài chính", Description = "Ngân hàng, công ty bảo hiểm, đầu tư.", IsDeleted = false, CreatedAt = DateTime.UtcNow }
                };

                var repo = _unitOfWork.GetRepository<BusinessType>();

                if (await repo.AnyAsync())
                {
                    return IdentityResult.Failed(new IdentityError { Description = "BusinessTypes already exist." });
                }

                await repo.AddRangeAsync(businessTypes);
                await _unitOfWork.SaveChangesAsync();

                return IdentityResult.Success;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to init business types: {ex.Message}");
            }
        }

        // Seed 3 user mặc định
        public async Task<IdentityResult> SeedUsersAsync()
        {
            // Seed admin
            await CreateUserWithRoleAsync("admin@example.com", "1", RolesHelper.Admin);

            // Seed employer
            var employerResult = await CreateUserWithRoleAsync("employer@example.com", "1", RolesHelper.Employer);
            if (employerResult.Succeeded)
            {
                var employerUser = await _userManager.FindByEmailAsync("employer@example.com");
                var employerRepo = _unitOfWork.GetRepository<Employer>();
                // Chọn BusinessTypeId mặc định, ví dụ 1
                var employer = new Employer
                {
                    UserId = employerUser.Id,
                    CompanyName = "Default Company",
                    BusinessTypeId = 1,
                    CompanyAddress = "Default Address",
                    TaxCode = "123456789",
                    CompanyDescription = "Default employer seeded",
                    Rating = 0,
                    TotalRatings = 0,
                    IsVerified = false,
                    CreatedAt = DateTime.UtcNow,
                    IsDeleted = false
                };
                await employerRepo.AddAsync(employer);
                await _unitOfWork.SaveChangesAsync();
            }

            // Seed worker
            var workerResult = await CreateUserWithRoleAsync("worker@example.com", "1", RolesHelper.Worker);
            if (workerResult.Succeeded)
            {
                var workerUser = await _userManager.FindByEmailAsync("worker@example.com");
                var workerRepo = _unitOfWork.GetRepository<Worker>();
                var worker = new Worker
                {
                    UserId = workerUser.Id,
                    Skills = "Default skills",
                    Bio = "Default bio",
                    Education = "Default education",
                    Experience = "Default experience",
                    Rating = 0,
                    TotalRatings = 0,
                    VerificationStatus = "PENDING",
                    CreatedAt = DateTime.UtcNow,
                    IsDeleted = false
                };
                await workerRepo.AddAsync(worker);
                await _unitOfWork.SaveChangesAsync();
            }

            return IdentityResult.Success;
        }

        // Tạo user mới và gán role (nếu đã tồn tại thì bỏ qua)
        public async Task<IdentityResult> CreateUserWithRoleAsync(string email, string password, string role)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
                return IdentityResult.Failed(new IdentityError { Description = "User already exists." });

            user = new User
            {
                FullName = email.Split('@')[0], // Lấy phần trước @ làm tên hiển thị
                UserName = email,
                Email = email,
                EmailConfirmed = true
            };
            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
                return result;

            if (await _roleManager.RoleExistsAsync(role))
            {
                await _userManager.AddToRoleAsync(user, role);
            }
            return IdentityResult.Success;
        }

        // Gán lại role cho user (mỗi user chỉ có 1 role)
        public async Task<IdentityResult> AssignRoleAsync(string email, string roleName)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return IdentityResult.Failed(new IdentityError { Description = "User not found." });

            if (!await _roleManager.RoleExistsAsync(roleName))
                return IdentityResult.Failed(new IdentityError { Description = "Role not found." });

            var currentRoles = await _userManager.GetRolesAsync(user);
            if (currentRoles.Any())
                await _userManager.RemoveFromRolesAsync(user, currentRoles);

            return await _userManager.AddToRoleAsync(user, roleName);
        }
    }
}
