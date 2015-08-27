using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using ClubMap.Models;
using Resources;

namespace ClubMap.Extensions
{
    public static class ExtendHtml
    {
        public static MvcHtmlString FormatMessage(this HtmlHelper html, MessageViewModel model)
        {
            if (model != null)
            {
                var sb = new StringBuilder();
                sb.Append("<strong>");
                switch (model.Type)
                {
                    case MessageViewModelType.info:
                        sb.Append(Strings.info);
                        break;
                    case MessageViewModelType.success:
                        sb.Append(Strings.success);
                        break;
                    case MessageViewModelType.warning:
                        sb.Append(Strings.warning);
                        break;
                    case MessageViewModelType.danger:
                        sb.Append(Strings.danger);
                        break;
                }
                sb.Append("</strong> ");
                sb.Append(model.Message);
                return new MvcHtmlString(sb.ToString());
            }

            return new MvcHtmlString("");
        }
    }
}