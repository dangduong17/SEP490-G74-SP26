using vn.edu.fpt.entity;
using vn.edu.fpt.repository;

namespace vn.edu.fpt.service
{
    public class SavedJobService : ISavedJobService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SavedJobService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<SavedJob>> GetAllSavedJobsAsync()
        {
            return await _unitOfWork.SavedJobs.GetAllAsync();
        }

        public async Task<SavedJob?> GetSavedJobByIdAsync(int id)
        {
            return await _unitOfWork.SavedJobs.GetByIdAsync(id);
        }

        public async Task<SavedJob?> SaveJobAsync(int userId, int jobId)
        {
            var savedJob = new SavedJob
            {
                UserId = userId,
                JobId = jobId,
                CreatedAt = DateTime.Now
            };

            await _unitOfWork.SavedJobs.AddAsync(savedJob);
            await _unitOfWork.CompleteAsync();

            return savedJob;
        }

        public async Task<bool> UnsaveJobAsync(int id)
        {
            var savedJob = await _unitOfWork.SavedJobs.GetByIdAsync(id);
            if (savedJob == null)
                return false;

            savedJob.IsDeleted = true;
            savedJob.UpdatedAt = DateTime.Now;

            _unitOfWork.SavedJobs.Update(savedJob);
            await _unitOfWork.CompleteAsync();

            return true;
        }

        public async Task<IEnumerable<SavedJob>> GetSavedJobsByUserAsync(int userId)
        {
            var savedJobs = await _unitOfWork.SavedJobs.GetAllAsync();
            return savedJobs.Where(s => s.UserId == userId && !s.IsDeleted);
        }
    }
}
