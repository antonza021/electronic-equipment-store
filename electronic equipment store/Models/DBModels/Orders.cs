using System.ComponentModel.DataAnnotations;

namespace electronic_equipment_store.Models.DBModels
{
    public class Orders
    {
        [Key] public int order_id { get; set; }
        public int customer_id { get; set; }
        public DateOnly order_date { get; set; }
        public decimal total_amount { get; set; }
    }
}
