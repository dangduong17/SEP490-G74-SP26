using vn.edu.fpt.dto;

namespace vn.edu.fpt.service
{
    public interface ICVService
    {
        Task<IEnumerable<CVDto>> GetAllCVsAsync();
        Task<CVDto?> GetCVByIdAsync(int id);
        Task<CVDto?> CreateCVAsync(CVDto cvDto);
        Task<CVDto?> UpdateCVAsync(int id, CVDto cvDto);
        Task<bool> DeleteCVAsync(int id);
        Task<IEnumerable<CVDto>> GetCVsByCandidateAsync(int candidateId);
    }
}
