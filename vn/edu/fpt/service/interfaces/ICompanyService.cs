using vn.edu.fpt.dto;

namespace vn.edu.fpt.service
{
    public interface ICompanyService
    {
        Task<IEnumerable<CompanyDto>> GetAllCompaniesAsync();
        Task<CompanyDto?> GetCompanyByIdAsync(int id);
        Task<CompanyDto?> CreateCompanyAsync(CompanyDto companyDto);
        Task<CompanyDto?> UpdateCompanyAsync(int id, CompanyDto companyDto);
        Task<bool> DeleteCompanyAsync(int id);
    }
}
