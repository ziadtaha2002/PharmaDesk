using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PharmaDesk.Models
{
    public class Medicine
    {
        [Key]
        public int Id { get; set; }

        [Required , StringLength(100, ErrorMessage = "Medicine name cannot exceed 100 characters")]
        public string Name { get; set; } = null!;

        [StringLength(150)]
        public string? GenericName { get; set; }

        [Required, StringLength(50)]
        public string Barcode { get; set; } = null!;

        [Required(ErrorMessage = "Category is required")]
        [ForeignKey("Category")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Unit is required")]
        [StringLength(20, ErrorMessage = "Unit cannot exceed 20 characters")]
        public string Unit { get; set; } = null!;

        [Required(ErrorMessage = "Selling price is required")]
        [Range(0.01, 999999.99, ErrorMessage = "Selling price must be greater than 0")]
        [DataType(DataType.Currency)]
        public decimal SellingPrice { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Low stock level must be at least 1")]
        public int LowStockLevel { get; set; } = 10;

        [Required]
        public bool IsActive { get; set; } = true;

        // Navigation properties
        public Category Category { get; set; } = null!;
        public ICollection<Batch> Batches { get; set; } = new List<Batch>();
        public ICollection<SaleItem> SaleItems { get; set; } = new List<SaleItem>();
        public ICollection<PurchaseItem> PurchaseItems { get; set; } = new List<PurchaseItem>();

        // Helper method to get total stock
        [NotMapped]
        public int TotalStock
        {
            get
            {
                return Batches
                    .Where(b => b.ExpiryDate > DateOnly.FromDateTime(DateTime.UtcNow))
                    .Sum(b => b.QuantityInStock);
            }
        }

        // Helper method to check if low stock
        [NotMapped]
        public bool IsLowStock => TotalStock <= LowStockLevel;

        // Helper method
        public override string ToString() => $"{Name} ({Barcode})";
    }
}
