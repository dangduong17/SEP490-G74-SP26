using AutoMapper;
using vn.edu.fpt.dto;
using vn.edu.fpt.entity;
using vn.edu.fpt.repository;

namespace vn.edu.fpt.service
{
    public class CompanyService : ICompanyService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CompanyService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CompanyDto>> GetAllCompaniesAsync()
        {
            var companies = await _unitOfWork.Companies.GetAllAsync();
            return _mapper.Map<IEnumerable<CompanyDto>>(companies);
        }

        public async Task<CompanyDto?> GetCompanyByIdAsync(int id)
        {
            var company = await _unitOfWork.Companies.GetByIdAsync(id);
            return company != null ? _mapper.Map<CompanyDto>(company) : null;
        }

        public async Task<CompanyDto?> CreateCompanyAsync(CompanyDto companyDto)
        {
            var company = _mapper.Map<Company>(companyDto);
            company.CreatedAt = DateTime.Now;

            await _unitOfWork.Companies.AddAsync(company);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<CompanyDto>(company);
        }

        public async Task<CompanyDto?> UpdateCompanyAsync(int id, CompanyDto companyDto)
        {
            var existingCompany = await _unitOfWork.Companies.GetByIdAsync(id);
            if (existingCompany == null)
                return null;

            _mapper.Map(companyDto, existingCompany);
            existingCompany.UpdatedAt = DateTime.Now;

            _unitOfWork.Companies.Update(existingCompany);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<CompanyDto>(existingCompany);
        }

        public async Task<bool> DeleteCompanyAsync(int id)
        {
            var company = await _unitOfWork.Companies.GetByIdAsync(id);
            if (company == null)
                return false;

            company.IsDeleted = true;
            company.UpdatedAt = DateTime.Now;

            _unitOfWork.Companies.Update(company);
            await _unitOfWork.CompleteAsync();

            return true;
        }
    }
}
