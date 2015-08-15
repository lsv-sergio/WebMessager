using System;

namespace WebMessenger.Models
{
    public class MessageModel
    {
        public int Id { get; set; }
        public string SenderName { get; set; }
        public string ReceiverName { get; set; }
        public string Message { get; set; }
        public string SendDate { get; set; }
    }
}
