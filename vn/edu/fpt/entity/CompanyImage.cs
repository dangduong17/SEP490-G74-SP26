using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace vn.edu.fpt.entity
{
    public class CompanyImage
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public int CompanyId { get; set; }
        
        [ForeignKey(nameof(CompanyId))]
        public Company Company { get; set; } = null!;
        
        [Required]
        [MaxLength(500)]
        public string ImageUrl { get; set; } = null!;
        
        [MaxLength(255)]
        public string? Caption { get; set; }
        
        public int DisplayOrder { get; set; } = 0;
        
        public DateTime CreatedAt { get; set; } = vn.edu.fpt.helper.DateTimeHelper.NowVietnam;
    }
}

