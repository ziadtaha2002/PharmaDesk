using System.ComponentModel.DataAnnotations;

namespace PharmaDesk.Models
{
    public class Supplier
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(30)]
        public string? Phone { get; set; }

        [StringLength(200)]
        public string? Address { get; set; }

        [StringLength(300)]
        public string? Notes { get; set; }

        public bool IsActive { get; set; } = true;

        // Navigation property
        public ICollection<Purchase> Purchases { get; set; } = new List<Purchase>();
    }
}