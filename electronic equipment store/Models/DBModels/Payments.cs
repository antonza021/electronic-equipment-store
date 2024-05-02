using System.ComponentModel.DataAnnotations;

namespace electronic_equipment_store.Models.DBModels
{
    public class Payments
    {
        [Key] public int payment_id { get; set; }
        public int order_id { get; set; }
        public DateOnly payment_date { get; set; }
        public string? payment_method { get; set; }
        public decimal amount { get; set; }
    }
}
