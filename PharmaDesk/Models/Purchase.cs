using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PharmaDesk.Models
{
    public class Purchase
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Supplier")]
        public int SupplierId { get; set; }

        [StringLength(50, ErrorMessage = "Invoice number cannot exceed 50 characters")]
        public string? InvoiceNumber { get; set; }

        [Required]
        public DateTime PurchaseDate { get; set; } = DateTime.UtcNow;

        [Required(ErrorMessage = "Total cost is required")]
        [Range(0.01, 999999999.99, ErrorMessage = "Total cost must be greater than 0")]
        [DataType(DataType.Currency)]
        public decimal TotalCost { get; set; }

        [Required]
        [ForeignKey("TeamMember")]
        public int TeamMemberId { get; set; }

        // Navigation properties
        public Supplier Supplier { get; set; } = null!;  
        public TeamMember TeamMember { get; set; } = null!; // we can use this to track who made the purchase
        public ICollection<PurchaseItem> PurchaseItems { get; set; } = new List<PurchaseItem>();

        // Helper method
        public override string ToString() => $"Purchase #{Id} from {Supplier.Name} on {PurchaseDate:yyyy-MM-dd}";
    }
}
