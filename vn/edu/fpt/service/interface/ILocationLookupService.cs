using vn.edu.fpt.dto;

namespace vn.edu.fpt.service.Interfaces
{
    public interface ILocationLookupService
    {
        Task<List<ProvinceLookupDto>> GetProvincesAsync();
        Task<List<WardLookupDto>> GetWardsByProvinceCodeAsync(int provinceCode);
    }
}
