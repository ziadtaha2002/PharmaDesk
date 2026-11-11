using System.ComponentModel.DataAnnotations;

namespace PharmaDesk.ViewModels
{
    // رأس الفاتورة
    public class PurchaseViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please choose a supplier.")]
        public int SupplierId { get; set; }

        [StringLength(50)]
        public string? InvoiceNumber { get; set; }

        public DateTime PurchaseDate { get; set; } = DateTime.Now;

        public List<Supplier> Suppliers { get; set; } = new List<Supplier>();
        public List<Medicine> Medicines { get; set; } = new List<Medicine>();

        public List<PurchaseItemViewModel> Items { get; set; } = new List<PurchaseItemViewModel>();
    }

    // بند واحد في الفاتورة
    public class PurchaseItemViewModel
    {
        [Required(ErrorMessage = "Please choose a medicine.")]
        public int MedicineId { get; set; }

        [Required(ErrorMessage = "Batch number is required.")]
        [StringLength(50)]
        public string BatchNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Expiry date is required.")]
        public DateOnly ExpiryDate { get; set; } = DateOnly.FromDateTime(DateTime.Today.AddYears(1));

        [Required(ErrorMessage = "Quantity is required.")]
        [Range(1, 100000, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Cost price is required.")]
        [Range(0.01, 999999, ErrorMessage = "Cost price must be greater than 0.")]
        public decimal CostPrice { get; set; }
    }
}