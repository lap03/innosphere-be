using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Repository.Data;
using Repository.Entities;
using Repository.Interfaces;
using Repository.Repositories;
using System.Reflection;

namespace innosphere_be.Configurations
{
    public static class ConfigurationService
    {
        public static void RegisterContextDb(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<InnoSphereDBContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), 
                options => options.MigrationsAssembly(typeof(InnoSphereDBContext).Assembly.FullName)));

            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<InnoSphereDBContext>()
                .AddDefaultTokenProviders();
        }

        public static void RegisterDI(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IGenericRepo<>), typeof(GenericRepo<>)); 
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
