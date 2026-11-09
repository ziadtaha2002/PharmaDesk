using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PharmaDesk.Models
{
    public class Sale
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Receipt number is required")]
        [StringLength(20, ErrorMessage = "Receipt number cannot exceed 20 characters")]
        public string ReceiptNumber { get; set; } =string.Empty; 

        [Required]
        public DateTime SaleDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Total amount is required")]
        [Range(0.01, 999999999.99, ErrorMessage = "Total amount must be greater than 0")]
        [DataType(DataType.Currency)]
        public decimal TotalAmount { get; set; }

        [Required(ErrorMessage = "Payment method is required")]
        [StringLength(20)]
        public string PaymentMethod { get; set; } = "Cash";

        [Required]
        [ForeignKey("TeamMember")]
        public int TeamMemberId { get; set; }

        // Navigation properties
        public TeamMember TeamMember { get; set; } = null!;
        public ICollection<SaleItem> SaleItems { get; set; } = new List<SaleItem>();

        // Helper method
        public override string ToString() => $"Receipt #{ReceiptNumber} - {TotalAmount:C} on {SaleDate:yyyy-MM-dd HH:mm}";
    }
}
