using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using ClubMap.DbModels.Sqlite;
using ClubMap.Models;

namespace ClubMap.DbModels
{
    public class Check
    {
        [Key]
        public string Id { get; set; }
        [Required]
        public DateTime Created { get; set; }
        [ForeignKey("HotelCode")]
        public Hotel Hotel { get; set; }
        public string HotelCode { get; set; }
        [ForeignKey("ClubCode")]
        public Club Club { get; set; }
        public string ClubCode { get; set; }
        [ForeignKey("RestaurantCode")]
        public Restaurant Restaurant { get; set; }
        public string RestaurantCode { get; set; }
        [ForeignKey("PubCode")]
        public Pub Pub { get; set; }
        public string PubCode { get; set; }
        [ForeignKey("CafeCode")]
        public Cafe Cafe { get; set; }
        public string CafeCode { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        [Required]
        public string ApplicationUserId { get; set; }

        public Check()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}