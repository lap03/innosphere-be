using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Repository.Entities;

namespace Repository.Data
{
    public class InnoSphereDBContext : IdentityDbContext<User, IdentityRole, string>
    {
        public InnoSphereDBContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //insert role
            modelBuilder.ApplyConfiguration(new RoleHardData());

            //rename AspNetUser and AspNetRole to Users and Roles
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<IdentityRole>().ToTable("Roles");
            modelBuilder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
        }
    }
}
