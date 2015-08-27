using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using ClubMap.Models;

namespace ClubMap.DbModels
{
    public class Message
    {
        [Key]
        public string Id { get; set; }
        public DateTime Created { get; set; }
        public string News { get; set; }
        [ForeignKey("ReceiverId")]
        public ApplicationUser Receiver { get; set; }
        public string ReceiverId { get; set; }
        [ForeignKey("SenderId")]
        public ApplicationUser Sender { get; set; }
        public string SenderId { get; set; }
        public Message()
        {
            Id = Guid.NewGuid().ToString();
        }

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(ReceiverId) &&
                   !string.IsNullOrEmpty(SenderId) &&
                   !string.IsNullOrEmpty(News);
        }
    }
}