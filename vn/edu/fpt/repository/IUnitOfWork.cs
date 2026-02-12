using vn.edu.fpt.entity;

namespace vn.edu.fpt.repository
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<User> Users { get; }
        IGenericRepository<Company> Companies { get; }
        IGenericRepository<Job> Jobs { get; }
        IGenericRepository<CV> CVs { get; }
        IGenericRepository<Application> Applications { get; }
        IGenericRepository<Skill> Skills { get; }
        IGenericRepository<JobCategory> JobCategories { get; }
        IGenericRepository<Location> Locations { get; }
        IGenericRepository<SubscriptionPlan> SubscriptionPlans { get; }
        IGenericRepository<Subscription> Subscriptions { get; }
        IGenericRepository<Payment> Payments { get; }
        IGenericRepository<Notification> Notifications { get; }
        IGenericRepository<SavedJob> SavedJobs { get; }

        Task<int> CompleteAsync();
    }
}
