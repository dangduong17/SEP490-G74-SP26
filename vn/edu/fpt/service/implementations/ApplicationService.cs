using AutoMapper;
using vn.edu.fpt.dto;
using vn.edu.fpt.entity;
using vn.edu.fpt.repository;

namespace vn.edu.fpt.service
{
    public class ApplicationService : IApplicationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ApplicationService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ApplicationDto>> GetAllApplicationsAsync()
        {
            var applications = await _unitOfWork.Applications.GetAllAsync();
            return _mapper.Map<IEnumerable<ApplicationDto>>(applications);
        }

        public async Task<ApplicationDto?> GetApplicationByIdAsync(int id)
        {
            var application = await _unitOfWork.Applications.GetByIdAsync(id);
            return application != null ? _mapper.Map<ApplicationDto>(application) : null;
        }

        public async Task<ApplicationDto?> CreateApplicationAsync(ApplicationDto applicationDto)
        {
            var application = _mapper.Map<Application>(applicationDto);
            application.AppliedDate = DateTime.Now;
            application.CreatedAt = DateTime.Now;
            application.Status = ApplicationStatus.Submitted;

            await _unitOfWork.Applications.AddAsync(application);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<ApplicationDto>(application);
        }

        public async Task<ApplicationDto?> UpdateApplicationAsync(int id, ApplicationDto applicationDto)
        {
            var existingApplication = await _unitOfWork.Applications.GetByIdAsync(id);
            if (existingApplication == null)
                return null;

            _mapper.Map(applicationDto, existingApplication);
            existingApplication.UpdatedAt = DateTime.Now;

            _unitOfWork.Applications.Update(existingApplication);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<ApplicationDto>(existingApplication);
        }

        public async Task<bool> DeleteApplicationAsync(int id)
        {
            var application = await _unitOfWork.Applications.GetByIdAsync(id);
            if (application == null)
                return false;

            application.IsDeleted = true;
            application.UpdatedAt = DateTime.Now;

            _unitOfWork.Applications.Update(application);
            await _unitOfWork.CompleteAsync();

            return true;
        }

        public async Task<IEnumerable<ApplicationDto>> GetApplicationsByJobAsync(int jobId)
        {
            var applications = await _unitOfWork.Applications.GetAllAsync();
            var jobApplications = applications.Where(a => a.JobId == jobId && !a.IsDeleted);
            return _mapper.Map<IEnumerable<ApplicationDto>>(jobApplications);
        }

        public async Task<IEnumerable<ApplicationDto>> GetApplicationsByCandidateAsync(int candidateId)
        {
            var applications = await _unitOfWork.Applications.GetAllAsync();
            var candidateApplications = applications.Where(a => a.CandidateId == candidateId && !a.IsDeleted);
            return _mapper.Map<IEnumerable<ApplicationDto>>(candidateApplications);
        }

        public async Task<ApplicationDto?> UpdateApplicationStatusAsync(int id, int status)
        {
            var application = await _unitOfWork.Applications.GetByIdAsync(id);
            if (application == null)
                return null;

            application.Status = (ApplicationStatus)status;
            application.UpdatedAt = DateTime.Now;

            _unitOfWork.Applications.Update(application);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<ApplicationDto>(application);
        }
    }
}
