using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PharmaDesk.Models
{
    public class ActivityLog
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("TeamMember")]
        public int TeamMemberId { get; set; }

        [Required(ErrorMessage = "Action is required")]
        [StringLength(50, ErrorMessage = "Action cannot exceed 50 characters")]
        public string Action { get; set; } = null!;

        [StringLength(300, ErrorMessage = "Details cannot exceed 300 characters")]
        public string? Details { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation property
        public TeamMember TeamMember { get; set; } = null!;

        // Helper method
        public override string ToString() => $"[{CreatedAt:yyyy-MM-dd HH:mm}] {TeamMember.FullName} - {Action}: {Details}";
    }
}
