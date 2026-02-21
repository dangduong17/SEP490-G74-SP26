using vn.edu.fpt.data;
using vn.edu.fpt.entity;

namespace vn.edu.fpt.repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly RJMSDbContext _context;

        public UnitOfWork(RJMSDbContext context)
        {
            _context = context;
            Candidates = new GenericRepository<Candidate>(_context);
            Recruiters = new GenericRepository<Recruiter>(_context);
            Admins = new GenericRepository<Admin>(_context);
            Users = new GenericRepository<User>(_context);
            Companies = new GenericRepository<Company>(_context);
            CompanyAddresses = new GenericRepository<CompanyAddress>(_context);
            Jobs = new GenericRepository<Job>(_context);
            CVs = new GenericRepository<CV>(_context);
            Applications = new GenericRepository<Application>(_context);
            Skills = new GenericRepository<Skill>(_context);
            JobCategories = new GenericRepository<JobCategory>(_context);
            Locations = new GenericRepository<Location>(_context);
            SubscriptionPlans = new GenericRepository<SubscriptionPlan>(_context);
            Subscriptions = new GenericRepository<Subscription>(_context);
            Payments = new GenericRepository<Payment>(_context);
            Notifications = new GenericRepository<Notification>(_context);
            SavedJobs = new GenericRepository<SavedJob>(_context);
        }

        public IGenericRepository<Candidate> Candidates { get; private set; }
        public IGenericRepository<Recruiter> Recruiters { get; private set; }
        public IGenericRepository<Admin> Admins { get; private set; }
        public IGenericRepository<User> Users { get; private set; }
        public IGenericRepository<Company> Companies { get; private set; }
        public IGenericRepository<CompanyAddress> CompanyAddresses { get; private set; }
        public IGenericRepository<Job> Jobs { get; private set; }
        public IGenericRepository<CV> CVs { get; private set; }
        public IGenericRepository<Application> Applications { get; private set; }
        public IGenericRepository<Skill> Skills { get; private set; }
        public IGenericRepository<JobCategory> JobCategories { get; private set; }
        public IGenericRepository<Location> Locations { get; private set; }
        public IGenericRepository<SubscriptionPlan> SubscriptionPlans { get; private set; }
        public IGenericRepository<Subscription> Subscriptions { get; private set; }
        public IGenericRepository<Payment> Payments { get; private set; }
        public IGenericRepository<Notification> Notifications { get; private set; }
        public IGenericRepository<SavedJob> SavedJobs { get; private set; }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
