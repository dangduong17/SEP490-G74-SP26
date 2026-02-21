using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace vn.edu.fpt.entity
{
    public class Admin
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public string UserId { get; set; } = null!;
        
        [ForeignKey(nameof(UserId))]
        public User User { get; set; } = null!;
        
        [MaxLength(255)]
        public string? FullName { get; set; }
        
        [MaxLength(20)]
        public string? Phone { get; set; }
        
        [MaxLength(500)]
        public string? Avatar { get; set; }
        
        [MaxLength(100)]
        public string? Department { get; set; }
        
        public DateTime CreatedAt { get; set; } = vn.edu.fpt.helper.DateTimeHelper.NowVietnam;
        
        public DateTime? UpdatedAt { get; set; }
    }
}

