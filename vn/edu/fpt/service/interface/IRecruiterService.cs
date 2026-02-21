using vn.edu.fpt.dto;

namespace vn.edu.fpt.service.Interfaces
{
    public interface IRecruiterService
    {
        Task<RecruiterDashboardViewModel?> GetDashboardAsync(string userId);
        Task<RecruiterProfileUpdateViewModel?> GetProfileAsync(string userId);
        Task<ServiceResult> UpdateProfileAsync(string userId, RecruiterProfileUpdateViewModel model);
    }
}
