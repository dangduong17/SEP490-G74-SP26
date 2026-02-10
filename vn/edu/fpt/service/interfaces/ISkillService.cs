using vn.edu.fpt.entity;

namespace vn.edu.fpt.service
{
    public interface ISkillService
    {
        Task<IEnumerable<Skill>> GetAllSkillsAsync();
        Task<Skill?> GetSkillByIdAsync(int id);
        Task<Skill?> CreateSkillAsync(Skill skill);
        Task<Skill?> UpdateSkillAsync(int id, Skill skill);
        Task<bool> DeleteSkillAsync(int id);
    }
}
