using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Resources;

namespace ClubMap.DbModels.Sqlite
{
    public class Restaurant : ModelBase
    {
        public string MondayOpening { get; set; }
        public string ThusdayOpening { get; set; }
        public string WednesdayOpening { get; set; }
        public string ThursdayOpening { get; set; }
        public string FridayOpening { get; set; }
        public string SaturdayOpening { get; set; }
        public string SundayOpening { get; set; }
        public string MondayClosing { get; set; }
        public string ThusdayClosing { get; set; }
        public string WednesdayClosing { get; set; }
        public string ThursdayClosing { get; set; }
        public string FridayClosing { get; set; }
        public string SaturdayClosing { get; set; }
        public string SundayClosing { get; set; }
        [Required(ErrorMessageResourceType = typeof(Strings), ErrorMessageResourceName = "this_field_is_required")]
        [DefaultValue(false)]
        public bool AvailableCoupons { get; set; }
        public FoodType FoodType { get; set; }
        public int? FoodTypeId { get; set; }
        public string PricesRange1 { get; set; }
        public string PricesRange2 { get; set; }
        public string CheckMenu { get; set; }
    }
}