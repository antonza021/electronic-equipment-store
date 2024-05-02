using System.ComponentModel.DataAnnotations;

namespace electronic_equipment_store.Models.DBModels
{
    public class Order_Items
    {
        [Key] public int order_item_id { get; set; }
        public int order_id { get; set; }
        public int product_id { get; set; }
        public int quantity { get; set; }
        public double unit_price { get; set; }
    }
}
