using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Resources;

namespace ClubMap.DbModels.Sqlite
{
    public class Hotel : ModelBase
    {
        [ForeignKey("HotelPriceId")]
        public Price HotelPrice { get; set; }
        public int? HotelPriceId { get; set; }
        [Required(ErrorMessageResourceType = typeof(Strings), ErrorMessageResourceName = "this_field_is_required")]
        public string Checkin { get; set; }
        [Required(ErrorMessageResourceType = typeof(Strings), ErrorMessageResourceName = "this_field_is_required")]
        public string Checkout { get; set; }
        [Required(ErrorMessageResourceType = typeof(Strings), ErrorMessageResourceName = "this_field_is_required")]
        public double Opinion { get; set; }
        [Required(ErrorMessageResourceType = typeof(Strings), ErrorMessageResourceName = "this_field_is_required")]
        public uint RoomPrice { get; set; }
        [Required(ErrorMessageResourceType = typeof(Strings), ErrorMessageResourceName = "this_field_is_required")]
        public uint Stars { get; set; }
        [Required(ErrorMessageResourceType = typeof(Strings), ErrorMessageResourceName = "this_field_is_required")]
        public double? Parking { get; set; }
        [Required(ErrorMessageResourceType = typeof(Strings), ErrorMessageResourceName = "this_field_is_required")]
        public double? Internet { get; set; }
        [Required(ErrorMessageResourceType = typeof(Strings), ErrorMessageResourceName = "this_field_is_required")]
        public string Kid { get; set; }
        [Required(ErrorMessageResourceType = typeof(Strings), ErrorMessageResourceName = "this_field_is_required")]
        public bool Animals { get; set; }
        public string PicturesLink { get; set; }
        [Required(ErrorMessageResourceType = typeof(Strings), ErrorMessageResourceName = "this_field_is_required")]
        [DefaultValue(false)]
        public bool AvailableCoupons { get; set; }
    }

    public enum Animals
    {
        Accepted,
        NotAccepted,
    }
}