using vn.edu.fpt.entity;

namespace vn.edu.fpt.service
{
    public interface ISavedJobService
    {
        Task<IEnumerable<SavedJob>> GetAllSavedJobsAsync();
        Task<SavedJob?> GetSavedJobByIdAsync(int id);
        Task<SavedJob?> SaveJobAsync(int userId, int jobId);
        Task<bool> UnsaveJobAsync(int id);
        Task<IEnumerable<SavedJob>> GetSavedJobsByUserAsync(int userId);
    }
}
