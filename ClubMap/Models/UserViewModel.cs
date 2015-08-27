using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ClubMap.DbModels;

namespace ClubMap.Models
{
    public class UserViewModel
    {
        public Image Icon { get; set; }
        public string UserName { get; set; }
        public string UserId { get; set; }
    }
}