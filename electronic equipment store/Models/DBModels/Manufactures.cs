using System.ComponentModel.DataAnnotations;

namespace electronic_equipment_store.Models.DBModels
{
    public class Manufactures
    {
        [Key] public int manufacturer_id { get; set; }
        public string? manufacturer_name { get; set; }
        public string? description { get; set;}
    }
}
