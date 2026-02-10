using AutoMapper;
using vn.edu.fpt.dto;
using vn.edu.fpt.entity;
using vn.edu.fpt.repository;

namespace vn.edu.fpt.service
{
    public class CVService : ICVService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CVService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CVDto>> GetAllCVsAsync()
        {
            var cvs = await _unitOfWork.CVs.GetAllAsync();
            return _mapper.Map<IEnumerable<CVDto>>(cvs);
        }

        public async Task<CVDto?> GetCVByIdAsync(int id)
        {
            var cv = await _unitOfWork.CVs.GetByIdAsync(id);
            return cv != null ? _mapper.Map<CVDto>(cv) : null;
        }

        public async Task<CVDto?> CreateCVAsync(CVDto cvDto)
        {
            var cv = _mapper.Map<CV>(cvDto);
            cv.CreatedAt = DateTime.Now;

            await _unitOfWork.CVs.AddAsync(cv);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<CVDto>(cv);
        }

        public async Task<CVDto?> UpdateCVAsync(int id, CVDto cvDto)
        {
            var existingCV = await _unitOfWork.CVs.GetByIdAsync(id);
            if (existingCV == null)
                return null;

            _mapper.Map(cvDto, existingCV);
            existingCV.UpdatedAt = DateTime.Now;

            _unitOfWork.CVs.Update(existingCV);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<CVDto>(existingCV);
        }

        public async Task<bool> DeleteCVAsync(int id)
        {
            var cv = await _unitOfWork.CVs.GetByIdAsync(id);
            if (cv == null)
                return false;

            cv.IsDeleted = true;
            cv.UpdatedAt = DateTime.Now;

            _unitOfWork.CVs.Update(cv);
            await _unitOfWork.CompleteAsync();

            return true;
        }

        public async Task<IEnumerable<CVDto>> GetCVsByCandidateAsync(int candidateId)
        {
            var cvs = await _unitOfWork.CVs.GetAllAsync();
            var candidateCVs = cvs.Where(c => c.CandidateId == candidateId && !c.IsDeleted);
            return _mapper.Map<IEnumerable<CVDto>>(candidateCVs);
        }
    }
}
