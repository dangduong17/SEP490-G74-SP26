using AutoMapper;
using Microsoft.EntityFrameworkCore;
using vn.edu.fpt.dto;
using vn.edu.fpt.entity;
using vn.edu.fpt.repository;

namespace vn.edu.fpt.service
{
    public class JobService : IJobService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public JobService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<JobDto>> GetAllJobsAsync()
        {
            var jobs = await _unitOfWork.Jobs.GetAllAsync();
            return _mapper.Map<IEnumerable<JobDto>>(jobs);
        }

        public async Task<JobDto?> GetJobByIdAsync(int id)
        {
            var job = await _unitOfWork.Jobs.GetByIdAsync(id);
            return job != null ? _mapper.Map<JobDto>(job) : null;
        }

        public async Task<JobDto?> CreateJobAsync(JobDto jobDto)
        {
            var job = _mapper.Map<Job>(jobDto);
            job.CreatedAt = DateTime.Now;
            job.Status = JobStatus.Active;

            await _unitOfWork.Jobs.AddAsync(job);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<JobDto>(job);
        }

        public async Task<JobDto?> UpdateJobAsync(int id, JobDto jobDto)
        {
            var existingJob = await _unitOfWork.Jobs.GetByIdAsync(id);
            if (existingJob == null)
                return null;

            _mapper.Map(jobDto, existingJob);
            existingJob.UpdatedAt = DateTime.Now;

            _unitOfWork.Jobs.Update(existingJob);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<JobDto>(existingJob);
        }

        public async Task<bool> DeleteJobAsync(int id)
        {
            var job = await _unitOfWork.Jobs.GetByIdAsync(id);
            if (job == null)
                return false;

            job.IsDeleted = true;
            job.UpdatedAt = DateTime.Now;

            _unitOfWork.Jobs.Update(job);
            await _unitOfWork.CompleteAsync();

            return true;
        }

        public async Task<IEnumerable<JobDto>> GetJobsByCompanyAsync(int companyId)
        {
            var jobs = await _unitOfWork.Jobs.GetAllAsync();
            var companyJobs = jobs.Where(j => j.CompanyId == companyId && !j.IsDeleted);
            return _mapper.Map<IEnumerable<JobDto>>(companyJobs);
        }

        public async Task<IEnumerable<JobDto>> GetJobsByRecruiterAsync(int recruiterId)
        {
            var jobs = await _unitOfWork.Jobs.GetAllAsync();
            var recruiterJobs = jobs.Where(j => j.RecruiterId == recruiterId && !j.IsDeleted);
            return _mapper.Map<IEnumerable<JobDto>>(recruiterJobs);
        }

        public async Task<IEnumerable<JobDto>> SearchJobsAsync(string keyword)
        {
            var jobs = await _unitOfWork.Jobs.GetAllAsync();
            var searchResults = jobs.Where(j => 
                !j.IsDeleted && 
                (j.Title.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                 j.Description.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                 j.Location.Contains(keyword, StringComparison.OrdinalIgnoreCase))
            );
            return _mapper.Map<IEnumerable<JobDto>>(searchResults);
        }
    }
}
