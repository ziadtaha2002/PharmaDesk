using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PharmaDesk.Models
{
    public class PurchaseItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Purchase")]
        public int PurchaseId { get; set; }

        [Required]
        [ForeignKey("Medicine")]
        public int MedicineId { get; set; }

        [Required(ErrorMessage = "Batch number is required")]
        [StringLength(50, ErrorMessage = "Batch number cannot exceed 50 characters")]
        public string BatchNumber { get; set; } = null!;

        [Required(ErrorMessage = "Expiry date is required")]
        public DateOnly ExpiryDate { get; set; }

        [Required(ErrorMessage = "Quantity is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Cost price is required")]
        [Range(0.01, 999999.99, ErrorMessage = "Cost price must be greater than 0")]
        [DataType(DataType.Currency)]
        public decimal CostPrice { get; set; }

        // Navigation properties
        public Purchase Purchase { get; set; } = null!;
        public Medicine Medicine { get; set; } = null!;

        // Helper method
        public override string ToString() => $"{Medicine.Name} - {Quantity} units @ {CostPrice}";
    }
}
