using System.ComponentModel.DataAnnotations;

namespace electronic_equipment_store.Models.DBModels
{
    public class Categories
    {
        [Key] public int category_id { get; set; }
        public string? category_name { get; set; }
    }
}
