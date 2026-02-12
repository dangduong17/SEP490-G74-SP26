using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using vn.edu.fpt.entity;

namespace vn.edu.fpt.data
{
    public class RJMSDbContext : IdentityDbContext<User>
    {
        public RJMSDbContext(DbContextOptions<RJMSDbContext> options) : base(options)
        {
        }

        // Profile tables
        public DbSet<Candidate> Candidates { get; set; }
        public DbSet<Recruiter> Recruiters { get; set; }
        public DbSet<Admin> Admins { get; set; }

        // Core tables
        public DbSet<Company> Companies { get; set; }
        public DbSet<CompanyAddress> CompanyAddresses { get; set; }
        public DbSet<CompanyImage> CompanyImages { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<CV> CVs { get; set; }
        public DbSet<Application> Applications { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<JobCategory> JobCategories { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<SubscriptionPlan> SubscriptionPlans { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        // Skill tables
        public DbSet<Skill> Skills { get; set; }
        public DbSet<JobSkill> JobSkills { get; set; }
        public DbSet<CandidateSkill> CandidateSkills { get; set; }

        // Profile detail tables
        public DbSet<Education> Educations { get; set; }
        public DbSet<WorkExperience> WorkExperiences { get; set; }

        // Relationship tables
        public DbSet<SavedJob> SavedJobs { get; set; }
        public DbSet<FollowedCompany> FollowedCompanies { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // User relationships
            builder.Entity<User>()
                .HasOne(u => u.Candidate)
                .WithOne(cp => cp.User)
                .HasForeignKey<Candidate>(cp => cp.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<User>()
                .HasOne(u => u.Recruiter)
                .WithOne(rp => rp.User)
                .HasForeignKey<Recruiter>(rp => rp.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<User>()
                .HasOne(u => u.Admin)
                .WithOne(ap => ap.User)
                .HasForeignKey<Admin>(ap => ap.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Application relationships - prevent cascade conflicts
            builder.Entity<Application>()
                .HasOne(a => a.Job)
                .WithMany(j => j.Applications)
                .HasForeignKey(a => a.JobId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Application>()
                .HasOne(a => a.Candidate)
                .WithMany(c => c.Applications)
                .HasForeignKey(a => a.CandidateId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Application>()
                .HasOne(a => a.CV)
                .WithMany(cv => cv.Applications)
                .HasForeignKey(a => a.CVId)
                .OnDelete(DeleteBehavior.NoAction);

            // CV relationship - prevent cascade conflicts
            builder.Entity<CV>()
                .HasOne(cv => cv.Candidate)
                .WithMany(c => c.CVs)
                .HasForeignKey(cv => cv.CandidateId)
                .OnDelete(DeleteBehavior.Cascade);

            // Job relationships - prevent cascade conflicts
            builder.Entity<Job>()
                .HasOne(j => j.Company)
                .WithMany(c => c.Jobs)
                .HasForeignKey(j => j.CompanyId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Job>()
                .HasOne(j => j.Recruiter)
                .WithMany(r => r.Jobs)
                .HasForeignKey(j => j.RecruiterId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Job>()
                .HasOne(j => j.CompanyAddress)
                .WithMany(ca => ca.Jobs)
                .HasForeignKey(j => j.CompanyAddressId)
                .OnDelete(DeleteBehavior.NoAction);

            // Education relationship
            builder.Entity<Education>()
                .HasOne(e => e.Candidate)
                .WithMany(c => c.Educations)
                .HasForeignKey(e => e.CandidateId)
                .OnDelete(DeleteBehavior.Cascade);

            // WorkExperience relationship
            builder.Entity<WorkExperience>()
                .HasOne(w => w.Candidate)
                .WithMany(c => c.WorkExperiences)
                .HasForeignKey(w => w.CandidateId)
                .OnDelete(DeleteBehavior.Cascade);

            // CompanyAddress relationship
            builder.Entity<CompanyAddress>()
                .HasOne(ca => ca.Company)
                .WithMany(c => c.Addresses)
                .HasForeignKey(ca => ca.CompanyId)
                .OnDelete(DeleteBehavior.Cascade);

            // CompanyImage relationship
            builder.Entity<CompanyImage>()
                .HasOne(ci => ci.Company)
                .WithMany(c => c.Images)
                .HasForeignKey(ci => ci.CompanyId)
                .OnDelete(DeleteBehavior.Cascade);

            // Recruiter relationship
            builder.Entity<Recruiter>()
                .HasOne(rp => rp.Company)
                .WithMany(c => c.Recruiters)
                .HasForeignKey(rp => rp.CompanyId)
                .OnDelete(DeleteBehavior.NoAction);

            // SubscriptionPlan relationship
            builder.Entity<Subscription>()
                .HasOne(s => s.Plan)
                .WithMany()
                .HasForeignKey(s => s.PlanId)
                .OnDelete(DeleteBehavior.NoAction);

            // Payment relationship
            builder.Entity<Payment>()
                .HasOne(p => p.Subscription)
                .WithMany()
                .HasForeignKey(p => p.SubscriptionId)
                .OnDelete(DeleteBehavior.Cascade);

            // Notification relationship
            builder.Entity<Notification>()
                .HasOne(n => n.User)
                .WithMany()
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // JobSkill composite key
            builder.Entity<JobSkill>()
                .HasKey(js => new { js.JobId, js.SkillId });

            builder.Entity<JobSkill>()
                .HasOne(js => js.Job)
                .WithMany(j => j.Skills)
                .HasForeignKey(js => js.JobId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<JobSkill>()
                .HasOne(js => js.Skill)
                .WithMany(s => s.Jobs)
                .HasForeignKey(js => js.SkillId)
                .OnDelete(DeleteBehavior.Cascade);

            // CandidateSkill composite key
            builder.Entity<CandidateSkill>()
                .HasKey(cs => new { cs.CandidateId, cs.SkillId });

            builder.Entity<CandidateSkill>()
                .HasOne(cs => cs.Candidate)
                .WithMany(c => c.Skills)
                .HasForeignKey(cs => cs.CandidateId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<CandidateSkill>()
                .HasOne(cs => cs.Skill)
                .WithMany(s => s.Candidates)
                .HasForeignKey(cs => cs.SkillId)
                .OnDelete(DeleteBehavior.Cascade);

            // SavedJob composite key
            builder.Entity<SavedJob>()
                .HasKey(sj => new { sj.CandidateId, sj.JobId });

            builder.Entity<SavedJob>()
                .HasOne(sj => sj.Candidate)
                .WithMany(c => c.SavedJobs)
                .HasForeignKey(sj => sj.CandidateId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<SavedJob>()
                .HasOne(sj => sj.Job)
                .WithMany(j => j.SavedByUsers)
                .HasForeignKey(sj => sj.JobId)
                .OnDelete(DeleteBehavior.NoAction);

            // FollowedCompany composite key
            builder.Entity<FollowedCompany>()
                .HasKey(fc => new { fc.CandidateId, fc.CompanyId });

            builder.Entity<FollowedCompany>()
                .HasOne(fc => fc.Candidate)
                .WithMany(c => c.FollowedCompanies)
                .HasForeignKey(fc => fc.CandidateId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<FollowedCompany>()
                .HasOne(fc => fc.Company)
                .WithMany(c => c.Followers)
                .HasForeignKey(fc => fc.CompanyId)
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.Entity<Job>()
                .HasIndex(j => j.Status);

            builder.Entity<Job>()
                .HasIndex(j => j.CreatedAt);

            builder.Entity<Application>()
                .HasIndex(a => a.Status);

            builder.Entity<Company>()
                .HasIndex(c => c.TaxCode)
                .IsUnique();

            builder.Entity<Candidate>()
                .HasIndex(cp => cp.UserId)
                .IsUnique();

            builder.Entity<Recruiter>()
                .HasIndex(rp => rp.UserId)
                .IsUnique();

            builder.Entity<Admin>()
                .HasIndex(ap => ap.UserId)
                .IsUnique();

            // Decimal precision
            builder.Entity<Job>()
                .Property(j => j.MinSalary)
                .HasPrecision(18, 2);

            builder.Entity<Job>()
                .Property(j => j.MaxSalary)
                .HasPrecision(18, 2);

            builder.Entity<Candidate>()
                .Property(c => c.CurrentSalary)
                .HasPrecision(18, 2);

            builder.Entity<Candidate>()
                .Property(c => c.ExpectedSalary)
                .HasPrecision(18, 2);

            builder.Entity<CompanyAddress>()
                .Property(ca => ca.Latitude)
                .HasPrecision(10, 7);

            builder.Entity<CompanyAddress>()
                .Property(ca => ca.Longitude)
                .HasPrecision(10, 7);

            builder.Entity<Education>()
                .Property(e => e.GPA)
                .HasPrecision(3, 2);
        }
    }
}
