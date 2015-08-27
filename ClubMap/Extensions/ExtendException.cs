using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Web;

namespace ClubMap.Extensions
{
    public static class ExtendException
    {
        public static String ToDetailedString(this Exception e)
        {
            var sb = new StringBuilder();
            sb.AppendLine(e.Message);
            if (e.InnerException != null)
                sb.AppendLine(e.InnerException.Message);
            if ((e is DbEntityValidationException) &&
                ((DbEntityValidationException)e).EntityValidationErrors != null)
            {
                foreach (var result in ((DbEntityValidationException)e).EntityValidationErrors)
                {
                    foreach (var error in result.ValidationErrors)
                        sb.AppendLine(error.PropertyName + " -> " + error.ErrorMessage);
                }
            }
            sb.Append(e.StackTrace);
            return sb.ToString().Trim();
        }
    }
}