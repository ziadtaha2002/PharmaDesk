using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PharmaDesk.Models
{
    public class TeamMember
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required, StringLength(200)]
        public string PinHash { get; set; } = string.Empty;

        [Required, StringLength(10)]
        public string Role { get; set; } = "Staff"; // Admin or Staff

        public bool IsActive { get; set; } = true;


        [Required]
        [Range(0, 5, ErrorMessage = "Failed PIN count cannot exceed 5")]
        public int FailedPinCount { get; set; } = 0;

        public DateTime? LockedUntil { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public ICollection<Sale> Sales { get; set; } = new List<Sale>();
        public ICollection<Purchase> Purchases { get; set; } = new List<Purchase>();
        public ICollection<ActivityLog> ActivityLogs { get; set; } = new List<ActivityLog>();

        // Helper methods
        public bool IsLockedOut()
        {
            return LockedUntil.HasValue && LockedUntil > DateTime.UtcNow;
        }

        public void LockAccount()
        {
            LockedUntil = DateTime.UtcNow.AddMinutes(5);
        }

        public void ResetFailedAttempts()
        {
            FailedPinCount = 0;
            LockedUntil = null;
        }

        public override string ToString() => $"{FullName} ({Role})";
    }
}
