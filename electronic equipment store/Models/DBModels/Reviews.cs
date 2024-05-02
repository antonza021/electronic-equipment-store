using System.ComponentModel.DataAnnotations;

namespace electronic_equipment_store.Models.DBModels
{
    public class Reviews
    {
        [Key] public int review_id { get; set; }
        public int product_id { get; set; }
        public int customer_id { get; set; }
        public string? review_text { get; set; }
        public int rating { get; set; }
    }
}
