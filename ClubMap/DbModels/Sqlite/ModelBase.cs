using System.ComponentModel.DataAnnotations;
using Resources;

namespace ClubMap.DbModels.Sqlite
{
    public class ModelBase
    {
        [Key]
        public string Code { get; set; }
        public Town Town { get; set; }
        [Required(ErrorMessageResourceType = typeof(Strings), ErrorMessageResourceName = "this_field_is_required")]
        public int TownId { get; set; }
        [Required(ErrorMessageResourceType = typeof(Strings), ErrorMessageResourceName = "this_field_is_required")]
        public string Name { get; set; }
        [Required(ErrorMessageResourceType = typeof(Strings), ErrorMessageResourceName = "this_field_is_required")]
        public string Address { get; set; }
        [Required(ErrorMessageResourceType = typeof(Strings), ErrorMessageResourceName = "this_field_is_required")]
        public double Latitude { get; set; }
        [Required(ErrorMessageResourceType = typeof(Strings), ErrorMessageResourceName = "this_field_is_required")]
        public double Longitude { get; set; }
        public string Phone { get; set; }
        [Required(ErrorMessageResourceType = typeof(Strings), ErrorMessageResourceName = "this_field_is_required")]
        public int Rank { get; set; }
    }
}