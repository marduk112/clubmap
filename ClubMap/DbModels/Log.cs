using System;
using System.ComponentModel.DataAnnotations;

namespace ClubMap.DbModels
{
    public class Log
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        [Required]
        public string Content { get; set; }
    }
}