using System.ComponentModel.DataAnnotations.Schema;

namespace vn.edu.fpt.entity
{
    public class JobSkill
    {
        public int JobId { get; set; }
        
        [ForeignKey(nameof(JobId))]
        public Job Job { get; set; } = null!;
        
        public int SkillId { get; set; }
        
        [ForeignKey(nameof(SkillId))]
        public Skill Skill { get; set; } = null!;
        
        public bool IsRequired { get; set; } = true;
    }
}
