using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ClubMap.Models;

namespace ClubMap.DbModels
{
    public class Like
    {
        [Key]
        public string Id { get; set; }
        [ForeignKey("LikedUserId")]
        public ApplicationUser LikedUser { get; set; }
        public string LikedUserId { get; set; }
        [ForeignKey("ApplicationUserId")]
        public ApplicationUser ApplicationUser { get; set; }
        public string ApplicationUserId { get; set; }
        public Like()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}