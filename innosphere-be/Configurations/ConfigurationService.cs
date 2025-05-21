using Microsoft.EntityFrameworkCore;
using Repository.Data;

namespace innosphere_be.Configurations
{
    public static class ConfigurationService
    {
        public static void RegisterContextDb(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<InnoSphereDBContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
                );
        }
    }
}
