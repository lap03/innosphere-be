using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Repository.Data;
using Repository.Entities;
using Repository.Interfaces;
using Repository.Repositories;
using Service.Interfaces;
using Service.Models.EmailModels;
using Service.Services;
using System.Reflection;
using System.Text;

namespace innosphere_be.Configurations
{
    public static class ConfigurationService
    {
        public static void SetupContextDb(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<InnoSphereDBContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                options => options.MigrationsAssembly(typeof(InnoSphereDBContext).Assembly.FullName)));

            services.AddIdentity<User, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 1;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;

                options.User.RequireUniqueEmail = true;
            })
                .AddEntityFrameworkStores<InnoSphereDBContext>()
                .AddDefaultTokenProviders()
                .AddTokenProvider<DataProtectorTokenProvider<User>>("REFRESHTOKENPROVIDER")
                .AddTokenProvider<SixDigitTokenProvider<User>>("SixDigitOTP");

            // Cấu hình lifespan cho từng provider
            services.Configure<DataProtectionTokenProviderOptions>(opt =>
            {
                opt.TokenLifespan = TimeSpan.FromHours(3); // Mặc định
            });
            services.Configure<DataProtectionTokenProviderOptions>("SixDigitOTP", opt =>
            {
                opt.TokenLifespan = TimeSpan.FromMinutes(5); // Riêng cho OTP
            });
        }

        public static void SetupDI(this IServiceCollection services, IConfiguration configuration)
        {
            // Add MailSettings
            services.Configure<EmailSettings>(configuration.GetSection("MailSettings"));

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IGenericRepo<>), typeof(GenericRepo<>));

            services.AddScoped<IEmailService, EmailService>();

            services.AddScoped<IWorkerService, WorkerService>();
            services.AddScoped<IEmployerService, EmployerService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IInitService, InitService>();
            services.AddScoped<IAuthService, AuthService>();

            services.AddScoped<ICityService, CityService>();
            services.AddScoped<IJobPostingService, JobPostingService>();
            services.AddScoped<IJobTagService, JobTagService>();
            services.AddScoped<IPaymentTypeService, PaymentTypeService>();
            services.AddScoped<ISubscriptionPackageService, SubscriptionPackageService>();
            services.AddScoped<IAdvertisementPackageService, AdvertisementPackageService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<ISocialLinkService, SocialLinkService>();
            services.AddScoped<IAdvertisementService, AdvertisementService>();
            services.AddScoped<ISubscriptionService, SubscriptionService>();
            services.AddScoped<IResumeService, ResumeService>();
            services.AddScoped<IRatingCriteriaService, RatingCriteriaService>();
            services.AddScoped<IJobApplicationService, JobApplicationService>();
            services.AddScoped<IBusinessTypeService, BusinessTypeService>();
            services.AddScoped<IUserService, UserService>();
        }

        public static void SetupJWT(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(x =>
                {
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidIssuer = configuration["JWT:Issuer"],
                        ValidAudience = configuration["JWT:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:SecretKey"]))
                    };
                })
                .AddGoogle(options =>
                {
                    options.ClientId = Environment.GetEnvironmentVariable("GOOGLE_CLIENT_ID");
                    options.ClientSecret = Environment.GetEnvironmentVariable("GOOGLE_CLIENT_SECRET");
                });

            services.AddAuthorization();
        }

        public static void SetupCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins",
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                               .AllowAnyMethod()
                               .AllowAnyHeader();
                    });
            });
        }

        public static void SetupRouting(this IServiceCollection services)
        {
            services.AddRouting(options => options.LowercaseUrls = true);
        }

        public static void SetupSwagger(this IServiceCollection services)
        {

            services.AddSwaggerGen(c =>
            {
                // Loại bỏ MetadataController của OData khỏi tài liệu Swagger
                c.DocInclusionPredicate((docName, apiDesc) =>
                {
                    // Loại bỏ tất cả các API từ Microsoft.AspNetCore.OData.Routing.Controllers
                    return !apiDesc.ActionDescriptor.DisplayName.Contains("MetadataController");
                });

                // Thêm quy ước tùy chỉnh cho schemaId
                c.CustomSchemaIds(type => type.FullName);

                // Xử lý các enum kiểu inline để tránh lỗi
                c.UseInlineDefinitionsForEnums();

                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "InnoSphere", Version = "v1" });
                var jwtSecurityScheme = new OpenApiSecurityScheme
                {
                    Name = "JWT Authentication",
                    Description = "Enter your JWT token in this field",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT"
                };

                c.AddSecurityDefinition("Bearer", jwtSecurityScheme);

                var securityRequirement = new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] {}
                        }
                    };

                c.AddSecurityRequirement(securityRequirement);

                //var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                //c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

                //// Cấu hình Swagger để sử dụng Newtonsoft.Json
                //c.UseAllOfForInheritance();

            });
        }
    }
}
