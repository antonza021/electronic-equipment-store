using System.ComponentModel.DataAnnotations;

namespace electronic_equipment_store.Models.DBModels
{
    public class Users
    {
        [Key] public int customer_id { get; set; }
        public string? customer_name { get; set; }
        public string? email { get; set;}
        public string? address { get; set; }
        public string? role { get; set; }
        public string? password { get; set; }
    }
}
