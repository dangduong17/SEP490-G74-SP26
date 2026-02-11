using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace vn.edu.fpt.entity
{
    public class CompanyAddress
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public int CompanyId { get; set; }
        
        [ForeignKey(nameof(CompanyId))]
        public Company Company { get; set; } = null!;
        
        [Required]
        [MaxLength(500)]
        public string Address { get; set; } = null!;
        
        [MaxLength(100)]
        public string? City { get; set; }
        
        [MaxLength(100)]
        public string? District { get; set; }
        
        [MaxLength(100)]
        public string? Ward { get; set; }
        
        [MaxLength(100)]
        public string? AddressType { get; set; }
        
        public bool IsHeadquarter { get; set; } = false;
        
        [MaxLength(20)]
        public string? Phone { get; set; }
        
        public decimal? Latitude { get; set; }
        
        public decimal? Longitude { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation properties
        public ICollection<Job> Jobs { get; set; } = new List<Job>();
    }
}
