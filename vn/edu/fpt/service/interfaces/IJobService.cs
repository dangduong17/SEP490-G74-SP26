using vn.edu.fpt.dto;
using vn.edu.fpt.entity;

namespace vn.edu.fpt.service
{
    public interface IJobService
    {
        Task<IEnumerable<JobDto>> GetAllJobsAsync();
        Task<JobDto?> GetJobByIdAsync(int id);
        Task<JobDto?> CreateJobAsync(JobDto jobDto);
        Task<JobDto?> UpdateJobAsync(int id, JobDto jobDto);
        Task<bool> DeleteJobAsync(int id);
        Task<IEnumerable<JobDto>> GetJobsByCompanyAsync(int companyId);
        Task<IEnumerable<JobDto>> GetJobsByRecruiterAsync(int recruiterId);
        Task<IEnumerable<JobDto>> SearchJobsAsync(string keyword);
    }
}
