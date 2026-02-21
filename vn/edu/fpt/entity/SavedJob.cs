using System.ComponentModel.DataAnnotations.Schema;

namespace vn.edu.fpt.entity
{
    public class SavedJob
    {
        public int CandidateId { get; set; }
        
        [ForeignKey(nameof(CandidateId))]
        public Candidate Candidate { get; set; } = null!;
        
        public int JobId { get; set; }
        
        [ForeignKey(nameof(JobId))]
        public Job Job { get; set; } = null!;
        
        public DateTime SavedAt { get; set; } = vn.edu.fpt.helper.DateTimeHelper.NowVietnam;
    }
}

