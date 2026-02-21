using System.ComponentModel.DataAnnotations.Schema;

namespace vn.edu.fpt.entity
{
    public class FollowedCompany
    {
        public int CandidateId { get; set; }
        
        [ForeignKey(nameof(CandidateId))]
        public Candidate Candidate { get; set; } = null!;
        
        public int CompanyId { get; set; }
        
        [ForeignKey(nameof(CompanyId))]
        public Company Company { get; set; } = null!;
        
        public DateTime FollowedAt { get; set; } = vn.edu.fpt.helper.DateTimeHelper.NowVietnam;
    }
}

