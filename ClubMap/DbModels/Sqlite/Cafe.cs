using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Resources;

namespace ClubMap.DbModels.Sqlite
{
    public class Cafe : ModelBase
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
        [ForeignKey("CafeTypeId")]
        public CafeType CafeType { get; set; }
        public int? CafeTypeId { get; set; }
        public double? Tea { get; set; }
        public double? BlackCoffee { get; set; }
        public double? WhiteCoffee { get; set; }
        public double? Espresso { get; set; }
        public double? Capuccino { get; set; }
        public double? Frappucino { get; set; }
        public double? DessertsLow { get; set; }
        public double? DessertsHigh { get; set; }
        public bool? HotDishes { get; set; }
    }
}