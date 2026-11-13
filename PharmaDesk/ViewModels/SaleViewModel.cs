using System.ComponentModel.DataAnnotations;

namespace PharmaDesk.ViewModels
{
    public class SaleViewModel
    {
        [Required]
        public string PaymentMethod { get; set; } = "Cash";     

        // قائمة العناصر في السلة
        public List<SaleItemViewModel> Items { get; set; } = new List<SaleItemViewModel>();
    }
                                
    public class SaleItemViewModel
    {
        [Required]
        public int MedicineId { get; set; }

        [Required]
        [Range(1, 10000, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; }

        // للعرض فقط (لا يتم إرسالها من الفورم، بل يأتي بها السيرفر)
        public string MedicineName { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
    }
}