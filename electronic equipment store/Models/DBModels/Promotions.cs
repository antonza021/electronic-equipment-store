using System.ComponentModel.DataAnnotations;

namespace electronic_equipment_store.Models.DBModels
{
    public class Promotions
    {
        [Key] public int promotion_id { get; set; }
        public string? promotion_name { get; set; }
        public DateTime start_date { get; set; }
        public DateTime end_date { get; set; }
        public decimal discount_percentage { get; set; }
    }
}
