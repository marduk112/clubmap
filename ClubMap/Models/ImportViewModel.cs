using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ClubMap.Models
{
    public class ImportViewModel
    {
        public HttpPostedFileBase FixedClubDataFile { get; set; }
        public HttpPostedFileBase VariableClubDataFile { get; set; }
    }
}