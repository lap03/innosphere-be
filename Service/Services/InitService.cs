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
                    // Restaurant & Food Service
                    new JobTag { TagName = "Waiter/Waitress", IsDeleted = false, CreatedAt = DateTime.UtcNow },
                    new JobTag { TagName = "Cashier", IsDeleted = false, CreatedAt = DateTime.UtcNow },
                    new JobTag { TagName = "Barista", IsDeleted = false, CreatedAt = DateTime.UtcNow },
                    new JobTag { TagName = "Bartender", IsDeleted = false, CreatedAt = DateTime.UtcNow },
                    new JobTag { TagName = "Chef", IsDeleted = false, CreatedAt = DateTime.UtcNow },
                    new JobTag { TagName = "Sous Chef", IsDeleted = false, CreatedAt = DateTime.UtcNow },
                    new JobTag { TagName = "Line Cook", IsDeleted = false, CreatedAt = DateTime.UtcNow },
                    new JobTag { TagName = "Kitchen Assistant", IsDeleted = false, CreatedAt = DateTime.UtcNow },
                    new JobTag { TagName = "Dishwasher", IsDeleted = false, CreatedAt = DateTime.UtcNow },
                    new JobTag { TagName = "Restaurant Manager", IsDeleted = false, CreatedAt = DateTime.UtcNow },
                    new JobTag { TagName = "Host/Hostess", IsDeleted = false, CreatedAt = DateTime.UtcNow },
                    new JobTag { TagName = "Food Runner", IsDeleted = false, CreatedAt = DateTime.UtcNow },
                    new JobTag { TagName = "Delivery Staff", IsDeleted = false, CreatedAt = DateTime.UtcNow },

                    // Retail
                    new JobTag { TagName = "Sales Associate", IsDeleted = false, CreatedAt = DateTime.UtcNow },
                    new JobTag { TagName = "Store Manager", IsDeleted = false, CreatedAt = DateTime.UtcNow },
                    new JobTag { TagName = "Stocker", IsDeleted = false, CreatedAt = DateTime.UtcNow },
                    new JobTag { TagName = "Inventory Clerk", IsDeleted = false, CreatedAt = DateTime.UtcNow },
                    new JobTag { TagName = "Customer Service", IsDeleted = false, CreatedAt = DateTime.UtcNow },
                    new JobTag { TagName = "Visual Merchandiser", IsDeleted = false, CreatedAt = DateTime.UtcNow },
                    new JobTag { TagName = "Security Guard", IsDeleted = false, CreatedAt = DateTime.UtcNow },

                    // General/Other
                    new JobTag { TagName = "Frontend Developer", IsDeleted = false, CreatedAt = DateTime.UtcNow },
                    new JobTag { TagName = "Backend Developer", IsDeleted = false, CreatedAt = DateTime.UtcNow },
                    new JobTag { TagName = "Mobile Developer", IsDeleted = false, CreatedAt = DateTime.UtcNow },
                    new JobTag { TagName = "Marketing Staff", IsDeleted = false, CreatedAt = DateTime.UtcNow },
                    new JobTag { TagName = "Accountant", IsDeleted = false, CreatedAt = DateTime.UtcNow },
                    new JobTag { TagName = "Receptionist", IsDeleted = false, CreatedAt = DateTime.UtcNow },
                    new JobTag { TagName = "Cleaner", IsDeleted = false, CreatedAt = DateTime.UtcNow },
                    new JobTag { TagName = "Driver", IsDeleted = false, CreatedAt = DateTime.UtcNow },
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
                    CompanySize = "50-100",
                    DateOfIncorporation = new DateTime(2010, 1, 1),
                    CompanyLogoUrl = "https://example.com/logo.png",
                    CompanyWebsite = "https://example.com",
                    CompanyCoverUrl = "https://example.com/cover.png",
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

        public async Task<IdentityResult> SeedSubscriptionPackagesAsync()
        {
            try
            {
                var subscriptionPackages = new List<SubscriptionPackage>
        {
            new SubscriptionPackage
            {
                PackageName = "Gói Cơ Bản",
                Description = "Gói đăng tin cơ bản, giới hạn 5 tin đăng",
                JobPostLimit = 5,
                Price = 100000,
                DurationDays = 30,
                IsDeleted = false,
                CreatedAt = DateTime.UtcNow
            },
            new SubscriptionPackage
            {
                PackageName = "Gói Tiêu Chuẩn",
                Description = "Gói đăng tin tiêu chuẩn, giới hạn 15 tin đăng",
                JobPostLimit = 15,
                Price = 250000,
                DurationDays = 90,
                IsDeleted = false,
                CreatedAt = DateTime.UtcNow
            },
            new SubscriptionPackage
            {
                PackageName = "Gói Cao Cấp",
                Description = "Gói đăng tin không giới hạn tin đăng",
                JobPostLimit = int.MaxValue,
                Price = 500000,
                DurationDays = 365,
                IsDeleted = false,
                CreatedAt = DateTime.UtcNow
            }
        };

                var repo = _unitOfWork.GetRepository<SubscriptionPackage>();

                if (await repo.AnyAsync())
                {
                    return IdentityResult.Failed(new IdentityError { Description = "Các gói đăng ký đã tồn tại." });
                }

                await repo.AddRangeAsync(subscriptionPackages);
                await _unitOfWork.SaveChangesAsync();

                return IdentityResult.Success;
            }
            catch (Exception ex)
            {
                throw new Exception($"Khởi tạo gói đăng ký thất bại: {ex.Message}");
            }
        }

        public async Task<IdentityResult> SeedAdvertisementPackagesAsync()
        {
            try
            {
                var advertisementPackages = new List<AdvertisementPackage>
        {
            new AdvertisementPackage
            {
                PackageName = "Gói Khởi Đầu",
                Description = "Gói quảng cáo khởi đầu với 1000 lượt hiển thị",
                Price = 150000,
                MaxImpressions = 1000,
                DurationDays = 30,
                AdPosition = "TOP", // Bắt buộc không null
                IsDeleted = false,
                CreatedAt = DateTime.UtcNow
            },
            new AdvertisementPackage
            {
                PackageName = "Gói Doanh Nghiệp",
                Description = "Gói quảng cáo doanh nghiệp với 5000 lượt hiển thị",
                Price = 400000,
                MaxImpressions = 5000,
                DurationDays = 90,
                AdPosition = "MIDDLE", // Bắt buộc không null
                IsDeleted = false,
                CreatedAt = DateTime.UtcNow
            },
            new AdvertisementPackage
            {
                PackageName = "Gói Doanh Nghiệp Cao Cấp",
                Description = "Gói quảng cáo không giới hạn lượt hiển thị",
                Price = 1000000,
                MaxImpressions = int.MaxValue,
                DurationDays = 365,
                AdPosition = "BOTTOM", // Bắt buộc không null
                IsDeleted = false,
                CreatedAt = DateTime.UtcNow
            }
        };

                var repo = _unitOfWork.GetRepository<AdvertisementPackage>();

                if (await repo.AnyAsync())
                {
                    return IdentityResult.Failed(new IdentityError { Description = "Các gói quảng cáo đã tồn tại." });
                }

                await repo.AddRangeAsync(advertisementPackages);
                await _unitOfWork.SaveChangesAsync();

                return IdentityResult.Success;
            }
            catch (Exception ex)
            {
                throw new Exception($"Khởi tạo gói quảng cáo thất bại: {ex.Message}");
            }
        }

        public async Task SeedEmployerSubscriptionsAndJobPostingsAsync()
        {
            // Lấy employer
            var employerRepo = _unitOfWork.GetRepository<Employer>();
            var subscriptionRepo = _unitOfWork.GetRepository<Subscription>();
            var jobPostingRepo = _unitOfWork.GetRepository<JobPosting>();

            var employerUser = await _userManager.FindByEmailAsync("employer@example.com");
            if (employerUser == null) return;

            var employer = (await employerRepo.GetAllAsync(e => e.UserId == employerUser.Id)).FirstOrDefault();
            if (employer == null) return;

            // Lấy gói đăng ký (giả sử đã seed 3 gói, lấy theo Id 1,2,3)
            var now = DateTime.UtcNow;
            var subscriptions = new List<Subscription>
            {
                 new Subscription
                {
                    EmployerId = employer.Id,
                    SubscriptionPackageId = 1,
                    StartDate = now.AddMonths(-2),
                    EndDate = now.AddMonths(-1),
                    IsActive = false,
                    IsDeleted = false,
                    CreatedAt = now.AddMonths(-2),
                    AmountPaid = 100000,
                    PaymentStatus = "PAID"
                },
                new Subscription
                {
                    EmployerId = employer.Id,
                    SubscriptionPackageId = 2,
                    StartDate = now.AddMonths(-1),
                    EndDate = now.AddDays(-7),
                    IsActive = false,
                    IsDeleted = false,
                    CreatedAt = now.AddMonths(-1),
                    AmountPaid = 250000,
                    PaymentStatus = "PAID"
                },
                new Subscription
                {
                    EmployerId = employer.Id,
                    SubscriptionPackageId = 3,
                    StartDate = now.AddDays(-5),
                    EndDate = now.AddMonths(1),
                    IsActive = true,
                    IsDeleted = false,
                    CreatedAt = now.AddDays(-5),
                    AmountPaid = 500000,
                    PaymentStatus = "PAID"
                }
            };
            // Thay vì AddRangeAsync → add từng cái để đảm bảo EF có Id đúng
            foreach (var sub in subscriptions)
            {
                await subscriptionRepo.AddAsync(sub);
            }

            // SaveChanges sau khi add hết → đảm bảo các sub có Id
            await _unitOfWork.SaveChangesAsync();

            // Lấy lại chính xác activeSubscription (sub có IsActive = true)
            // Do mình biết sub3 là active → có thể lấy subscriptions.First(s => s.IsActive).Id
            var activeSubscription = await subscriptionRepo.GetByIdAsync(subscriptions.First(s => s.IsActive).Id);

            if (activeSubscription == null) return;
            //await subscriptionRepo.AddRangeAsync(subscriptions);
            //await _unitOfWork.SaveChangesAsync();

            //var activeSubscription = (await subscriptionRepo.GetAllAsync(s =>
            //    s.EmployerId == employer.Id && s.IsActive)).FirstOrDefault();
            if (activeSubscription == null)
            {
                Console.WriteLine("❌ Active subscription not found, skipping JobPostings seeding.");
                return;
            }
            else
            {
                Console.WriteLine($"✅ Active subscription found, Id = {activeSubscription.Id}");
            }

            // Lấy cityId hợp lệ
            var cityRepo = _unitOfWork.GetRepository<City>();
            var city = (await cityRepo.GetAllAsync()).FirstOrDefault();
            if (city == null) return;
            int cityId = city.Id;

            // Tạo 10 job posting
            var jobPostings = new List<JobPosting>();
            for (int i = 1; i <= 10; i++)
            {
                jobPostings.Add(new JobPosting
                {
                    EmployerId = employer.Id,
                    SubscriptionId = activeSubscription.Id,
                    CityId = cityId,
                    Title = $"Job Title {i}",
                    Description = $"Sample job description {i}",
                    Location = "Sample Location",
                    StartTime = now.AddDays(i),
                    EndTime = now.AddDays(i + 7),
                    HourlyRate = 100000 + i * 10000,
                    JobType = "FullTime",
                    Requirements = "Sample requirements",
                    PostedAt = now.AddDays(-i),
                    ExpiresAt = now.AddDays(i + 30),
                    Status = "APPROVED",
                    IsUrgent = i % 2 == 0,
                    IsHighlighted = i % 3 == 0,
                    ViewsCount = i * 10,
                    ApplicationsCount = i * 2,
                    IsDeleted = false,
                    CreatedAt = now.AddDays(-i)
                });
            }

            await jobPostingRepo.AddRangeAsync(jobPostings);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
