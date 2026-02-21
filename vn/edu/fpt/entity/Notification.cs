using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace vn.edu.fpt.entity
{
    public class Notification
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public string UserId { get; set; } = null!;
        
        [ForeignKey(nameof(UserId))]
        public User User { get; set; } = null!;
        
        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = null!;
        
        public string Message { get; set; } = null!;
        
        public bool IsRead { get; set; } = false;
        
        public DateTime CreatedAt { get; set; } = vn.edu.fpt.helper.DateTimeHelper.NowVietnam;
        
        [MaxLength(50)]
        public string? Type { get; set; }
        
        [MaxLength(500)]
        public string? Link { get; set; }
    }
}

