using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PharmaDesk.Models
{
    public class Batch
    {
        public int Id { get; set; }

        [Required]
        public int MedicineId { get; set; }
        [ForeignKey("MedicineId")]
        public Medicine? Medicine { get; set; }

        [Required, StringLength(50)]
        public string BatchNumber { get; set; } = string.Empty;

        [Required]
        public DateOnly ExpiryDate { get; set; }

        public int QuantityInStock { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal CostPrice { get; set; }

        public int? PurchaseItemId { get; set; }
        [ForeignKey("PurchaseItemId")]
        public PurchaseItem? PurchaseItem { get; set; }

        // Navigation property
        public ICollection<SaleItem> SaleItems { get; set; } = new List<SaleItem>();
    }
}