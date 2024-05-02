using System.ComponentModel.DataAnnotations;

namespace electronic_equipment_store.Models.DBModels
{
    public class Products
    {
        [Key] public int product_id { get; set; }
        public string? product_name { get; set; }
        public string? description { get; set; }
        public decimal price { get; set; }
        public int category_id { get; set; }
        public int manufacturer_id { get; set; }
        public int stock_quantity { get; set; }
    }
}
