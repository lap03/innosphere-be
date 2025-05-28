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
        public DbSet<Worker> Workers { get; set; }
        public DbSet<Employer> Employers { get; set; }
        public DbSet<JobPosting> JobPostings { get; set; }
        public DbSet<JobApplication> JobApplications { get; set; }
        public DbSet<JobTag> JobTags { get; set; }
        public DbSet<JobPostingTag> JobPostingTags { get; set; }
        public DbSet<Resume> Resumes { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<SocialLink> SocialLinks { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<SubscriptionPackage> SubscriptionPackages { get; set; }
        public DbSet<Advertisement> Advertisements { get; set; }
        public DbSet<AdvertisementPackage> AdvertisementPackages { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<PaymentType> PaymentTypes { get; set; }

        public DbSet<EmployerRating> EmployerRatings { get; set; }
        public DbSet<EmployerRatingCriteria> EmployerRatingCriterias { get; set; }
        public DbSet<WorkerRating> WorkerRatings { get; set; }
        public DbSet<WorkerRatingCriteria> WorkerRatingCriterias { get; set; }
        public DbSet<RatingCriteria> RatingCriterias { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //insert role
            modelBuilder.ApplyConfiguration(new RoleHardData());

            //rename AspNetUser and AspNetRole to Users and Roles
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<IdentityRole>().ToTable("Roles");
            modelBuilder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
            // Disable cascading delete globally
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var fk in entityType.GetForeignKeys())
                {
                    if (!fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade)
                    {
                        fk.DeleteBehavior = DeleteBehavior.Restrict;
                    }
                }
            }

            // Cascade for Employer relationships
            modelBuilder.Entity<Employer>()
                .HasMany(e => e.JobPostings)
                .WithOne(j => j.Employer)
                .HasForeignKey(j => j.EmployerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Employer>()
                .HasMany(e => e.Subscriptions)
                .WithOne(s => s.Employer)
                .HasForeignKey(s => s.EmployerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Employer>()
                .HasMany(e => e.Payments)
                .WithOne(p => p.Employer)
                .HasForeignKey(p => p.EmployerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Employer>()
                .HasMany(e => e.Advertisements)
                .WithOne(a => a.Employer)
                .HasForeignKey(a => a.EmployerId)
                .OnDelete(DeleteBehavior.Cascade);

            // Worker - User (1-1)
            modelBuilder.Entity<Worker>()
                .HasOne(w => w.User)
                .WithOne(u => u.Worker)
                .HasForeignKey<Worker>(w => w.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Worker>()
                .HasIndex(w => w.UserId)
                .IsUnique();

            // Employer - User (1-1)
            modelBuilder.Entity<Employer>()
                .HasOne(e => e.User)
                .WithOne(u => u.Employer)
                .HasForeignKey<Employer>(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Employer>()
                .HasIndex(e => e.UserId)
                .IsUnique();

            // Payment - Advertisement (1-1 optional)
            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Advertisement)
                .WithOne()
                .HasForeignKey<Payment>(p => p.AdvertisementId)
                .OnDelete(DeleteBehavior.Restrict);

            // JobApplication - EmployerRating (1-1)
            modelBuilder.Entity<EmployerRating>()
                .HasOne(er => er.JobApplication)
                .WithOne()
                .HasForeignKey<EmployerRating>(er => er.JobApplicationId)
                .OnDelete(DeleteBehavior.Restrict);

            // JobApplication - WorkerRating (1-1)
            modelBuilder.Entity<WorkerRating>()
                .HasOne(wr => wr.JobApplication)
                .WithOne()
                .HasForeignKey<WorkerRating>(wr => wr.JobApplicationId)
                .OnDelete(DeleteBehavior.Restrict);

            // RatingCriteria - WorkerRatingCriteria (1-n)
            modelBuilder.Entity<WorkerRatingCriteria>()
                .HasOne(wrc => wrc.WorkerRating)
                .WithMany(wr => wr.RatingCriterias)
                .HasForeignKey(wrc => wrc.WorkerRatingId);

            modelBuilder.Entity<WorkerRatingCriteria>()
                .HasOne(wrc => wrc.RatingCriteria)
                .WithMany(rc => rc.WorkerRatingCriterias)
                .HasForeignKey(wrc => wrc.RatingCriteriaId);

            // RatingCriteria - EmployerRatingCriteria (1-n)
            modelBuilder.Entity<EmployerRatingCriteria>()
                .HasOne(erc => erc.EmployerRating)
                .WithMany(er => er.RatingCriterias)
                .HasForeignKey(erc => erc.EmployerRatingId);

            modelBuilder.Entity<EmployerRatingCriteria>()
                .HasOne(erc => erc.RatingCriteria)
                .WithMany(rc => rc.EmployerRatingCriterias)
                .HasForeignKey(erc => erc.RatingCriteriaId);
        }
    }
}
