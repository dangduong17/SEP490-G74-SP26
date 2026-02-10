using vn.edu.fpt.entity;
using vn.edu.fpt.repository;

namespace vn.edu.fpt.service
{
    public class SkillService : ISkillService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SkillService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Skill>> GetAllSkillsAsync()
        {
            return await _unitOfWork.Skills.GetAllAsync();
        }

        public async Task<Skill?> GetSkillByIdAsync(int id)
        {
            return await _unitOfWork.Skills.GetByIdAsync(id);
        }

        public async Task<Skill?> CreateSkillAsync(Skill skill)
        {
            skill.CreatedAt = DateTime.Now;
            await _unitOfWork.Skills.AddAsync(skill);
            await _unitOfWork.CompleteAsync();
            return skill;
        }

        public async Task<Skill?> UpdateSkillAsync(int id, Skill skill)
        {
            var existingSkill = await _unitOfWork.Skills.GetByIdAsync(id);
            if (existingSkill == null)
                return null;

            existingSkill.Name = skill.Name;
            existingSkill.Description = skill.Description;
            existingSkill.UpdatedAt = DateTime.Now;

            _unitOfWork.Skills.Update(existingSkill);
            await _unitOfWork.CompleteAsync();

            return existingSkill;
        }

        public async Task<bool> DeleteSkillAsync(int id)
        {
            var skill = await _unitOfWork.Skills.GetByIdAsync(id);
            if (skill == null)
                return false;

            skill.IsDeleted = true;
            skill.UpdatedAt = DateTime.Now;

            _unitOfWork.Skills.Update(skill);
            await _unitOfWork.CompleteAsync();

            return true;
        }
    }
}
