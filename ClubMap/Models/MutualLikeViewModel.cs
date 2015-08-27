using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClubMap.Models
{
    public class MutualLikeViewModel
    {
        //key is user id and value is user name
        public Dictionary<string, string> UsersDictionary { get; set; }

        public MutualLikeViewModel()
        {
            UsersDictionary = new Dictionary<string, string>();
        }
    }
}