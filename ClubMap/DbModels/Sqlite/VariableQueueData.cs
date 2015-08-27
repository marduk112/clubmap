using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClubMap.DbModels.Sqlite
{
    public class VariableQueueData
    {
        public int Id { get; set; }
        public int? BarTime { get; set; }
        public string Hour { get; set; }
        public string WomanPercent { get; set; }
        public string ManPercent { get; set; }
        public int? QueueTime { get; set; }
        public int? People { get; set; }
        [ForeignKey("DayId")]
        public Day Day { get; set; }
        public int? DayId { get; set; }
        //One of this id will be not null
        [ForeignKey("CafeCode")]
        public Cafe Cafe { get; set; }
        public string CafeCode { get; set; }
        [ForeignKey("ClubCode")]
        public Club Club { get; set; }
        public string ClubCode { get; set; }
        [ForeignKey("HotelCode")]
        public Hotel Hotel { get; set; }
        public string HotelCode { get; set; }
        [ForeignKey("PubCode")]
        public Pub Pub { get; set; }
        public string PubCode { get; set; }
        [ForeignKey("RestaurantCode")]
        public Restaurant Restaurant { get; set; }
        public string RestaurantCode { get; set; }
        [MaxLength(1)]
        public string PeopleArrow { get; set; }
        [MaxLength(1)]
        public string QueueArrow { get; set; }
        
    }

    public class Day
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}