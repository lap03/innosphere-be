using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository.Data
{
    public class RoleHardData : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            // Tạo các Role mặc định
            builder.HasData(
                new IdentityRole
                {
                    Id = "e5df0673-2801-41c1-924c-8a84ee3aaa70",
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
            new IdentityRole
            {
                Id = "b1a290ef-4467-4339-9931-218d8aeb46c0",
                Name = "Worker",
                NormalizedName = "WORKER"
            },
            new IdentityRole
            {
                Id = "e450c7fa-cb9b-40b0-b5f2-7ad083eefd63",
                Name = "Employer",
                NormalizedName = "EMPLOYER"
            }
            );
        }
    }
}
