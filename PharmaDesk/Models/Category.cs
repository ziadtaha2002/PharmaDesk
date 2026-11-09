using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PharmaDesk.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(60)]
        public string Name { get; set; } = string.Empty;

        // Navigation property
        public ICollection<Medicine> Medicines { get; set; } = new List<Medicine>();

        // Helper method
        public override string ToString() => Name;
    }
}
