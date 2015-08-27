using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClubMap.Common
{
    public class Constants
    {
        public static Dictionary<string, int> Days
        {
            get
            {
                return _days ?? (_days = new Dictionary<string, int>
                {
                    {"Monday", 1},
                    {"Thusday", 2},
                    {"Wednesday", 3},
                    {"Thursday", 4},
                    {"Friday", 5},
                    {"Saturday", 6},
                    {"Sunday", 7}
                });
            }
        }

        public enum UserRole
        {
            Admin,
        }

        private static Dictionary<string, int> _days;
    }
}