using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Resources;

namespace ClubMap.DbModels.Sqlite
{
    public class Club : ModelBase
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
        public MusicKind MusicKind { get; set; }
        public int? MusicKindId { get; set; }
        public AgeKind AgeKind { get; set; }
        public int? AgeKindId { get; set; }
        [ForeignKey("EnterPriceMondayId")]
        public EnterPriceDay EnterPriceMonday { get; set; }
        public int? EnterPriceMondayId { get; set; }
        [ForeignKey("EnterPriceThusdayId")]
        public EnterPriceDay EnterPriceThusday { get; set; }
        public int? EnterPriceThusdayId { get; set; }
        [ForeignKey("EnterPriceWednesdayId")]
        public EnterPriceDay EnterPriceWednesday { get; set; }
        public int? EnterPriceWednesdayId { get; set; }
        [ForeignKey("EnterPriceThursdayId")]
        public EnterPriceDay EnterPriceThursday { get; set; }
        public int? EnterPriceThursdayId { get; set; }
        [ForeignKey("EnterPriceFridayId")]
        public EnterPriceDay EnterPriceFriday { get; set; }
        public int? EnterPriceFridayId { get; set; }
        [ForeignKey("EnterPriceSaturdayId")]
        public EnterPriceDay EnterPriceSaturday { get; set; }
        public int? EnterPriceSaturdayId { get; set; }
        [ForeignKey("EnterPriceSundayId")]
        public EnterPriceDay EnterPriceSunday { get; set; }
        public int? EnterPriceSundayId { get; set; }
        public int? CoatroomPrice { get; set; }
        public double? AverageAge { get; set; }
        [Required(ErrorMessageResourceType = typeof(Strings), ErrorMessageResourceName = "this_field_is_required")]
        [DefaultValue(false)]
        public bool AvailableCoupons { get; set; }
        public double? Juice { get; set; }
        public double? BeerG { get; set; }
        public double? BeerBootle { get; set; }
        public double? VS { get; set; }
        public double? VB { get; set; }
        public double? VBJ { get; set; }
        public double? Drink1 { get; set; }
        public double? Drink2 { get; set; }
        public double? Drink3 { get; set; }
    }

    public enum AvailableCoupons
    {
        Yes,
        No
    }
}