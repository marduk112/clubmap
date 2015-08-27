using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClubMap.Models;

namespace ClubMap.Controllers
{
    public class WebController : Controller
    {
        public string Format(string format, object obj)
        {
            return string.Format(format, obj);
        }

        protected void SetMessageViewModel(string message, MessageViewModelType type = MessageViewModelType.info)
        {
            TempData_Message = new MessageViewModel(message, type);
        }

        private MessageViewModel TempData_Message
        {
            set
            {
                TempData["MessageViewModel"] = value;
            }
            get
            {
                return
                    TempData["MessageViewModel"] != null ?
                    (MessageViewModel)TempData["MessageViewModel"] :
                    null;
            }
        }
    }
}