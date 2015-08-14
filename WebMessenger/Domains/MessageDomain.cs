using System;

namespace WebMessenger.Domains
{
    public class MessageDomain
    {
        public int Id { get; set; }
        public UserDomain Sender { get; set; }
        public UserDomain Receiver { get; set; }
        public string Message { get; set; }
        public DateTime SendDate { get; set; }
    }
}
