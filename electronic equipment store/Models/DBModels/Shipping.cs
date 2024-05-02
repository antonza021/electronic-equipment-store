using System.ComponentModel.DataAnnotations;

namespace electronic_equipment_store.Models.DBModels
{
    public class Shipping
    {
        [Key] public int shipping_id { get; set; }
        public int order_id { get; set; }
        public DateOnly shipping_date { get; set; }
        public string? shipping_address { get; set; }
    }
}
