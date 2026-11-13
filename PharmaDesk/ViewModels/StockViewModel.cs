namespace PharmaDesk.ViewModels
{
    public class StockViewModel
    {
        public int MedicineId { get; set; }
        public string MedicineName { get; set; } = string.Empty;
        public string GenericName { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Unit { get; set; } = string.Empty;

        // مجموع الكميات في كل الدفعات غير المنتهية
        public int TotalQuantity { get; set; }

        // الحد الأدنى الذي نبهنا عليه
        public int LowStockLevel { get; set; }

        // هل الكمية منخفضة؟
        public bool IsLowStock => TotalQuantity <= LowStockLevel;

        // هل الدواء منتهي الصلاحية (لا يوجد لديه أي دفعة صالحة)؟
        public bool IsExpired => TotalQuantity == 0;
    }
}