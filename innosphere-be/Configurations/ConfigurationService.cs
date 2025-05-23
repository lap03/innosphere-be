using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.Entities;

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
        }
    }
}
