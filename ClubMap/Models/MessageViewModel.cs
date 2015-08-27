using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClubMap.Models
{
    public class MessageViewModel
    {
        public MessageViewModelType Type { get; private set; }
        public string Message { get; private set; }

        public MessageViewModel(string message, MessageViewModelType type = MessageViewModelType.info)
        {
            this.Type = type;
            this.Message = message;
        }
    }

    public enum MessageViewModelType
    {
        success,
        info,
        warning,
        danger
    }
}