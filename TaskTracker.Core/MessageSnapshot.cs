using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskTracker.Core
{
    public class MessageSnapshot
    {
        public PublicUserInfo? SenderUserInfo { get; set; }
        public string? Message { get; set; }
        public string? ReceiverNickname { get; set; }
        public string? SentDate { get; set; }
        public bool IsPersonal { get; set; }
    }
}
