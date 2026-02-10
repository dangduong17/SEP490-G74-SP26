using vn.edu.fpt.dto;

namespace vn.edu.fpt.service
{
    public interface IApplicationService
    {
        Task<IEnumerable<ApplicationDto>> GetAllApplicationsAsync();
        Task<ApplicationDto?> GetApplicationByIdAsync(int id);
        Task<ApplicationDto?> CreateApplicationAsync(ApplicationDto applicationDto);
        Task<ApplicationDto?> UpdateApplicationAsync(int id, ApplicationDto applicationDto);
        Task<bool> DeleteApplicationAsync(int id);
        Task<IEnumerable<ApplicationDto>> GetApplicationsByJobAsync(int jobId);
        Task<IEnumerable<ApplicationDto>> GetApplicationsByCandidateAsync(int candidateId);
        Task<ApplicationDto?> UpdateApplicationStatusAsync(int id, int status);
    }
}
