using PharmaDesk.Models;
using System.ComponentModel.DataAnnotations;

namespace PharmaDesk.ViewModels
{
    public class MedicineViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Medicine name is required.")]
        [StringLength(150)]
        public string Name { get; set; } = string.Empty;

        [StringLength(150)]
        public string? GenericName { get; set; }

        [Required(ErrorMessage = "Barcode is required.")]
        [StringLength(50)]
        public string Barcode { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please choose a category.")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Unit is required.")]
        [StringLength(20)]
        public string Unit { get; set; } = string.Empty; 

        [Required(ErrorMessage = "Selling price is required.")]
        [Range(0.01, 999999, ErrorMessage = "Price must be greater than 0.")]
        public decimal SellingPrice { get; set; }

        [Required(ErrorMessage = "Low stock level is required.")]
        [Range(0, 100000)]
        public int LowStockLevel { get; set; } = 10;

        public bool IsActive { get; set; } = true;

        // هذه ليست حقولاً في قاعدة البيانات، بل نستخدمها لعرض القوائم المنسدلة
        public List<Category> Categories { get; set; } = new List<Category>();
    }
}
    