using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace vn.edu.fpt.entity
{
    public class CandidateSkill
    {
        public int CandidateId { get; set; }
        
        [ForeignKey(nameof(CandidateId))]
        public Candidate Candidate { get; set; } = null!;
        
        public int SkillId { get; set; }
        
        [ForeignKey(nameof(SkillId))]
        public Skill Skill { get; set; } = null!;
        
        [MaxLength(50)]
        public string? Level { get; set; }
        
        public int? YearsOfExperience { get; set; }
    }
}
