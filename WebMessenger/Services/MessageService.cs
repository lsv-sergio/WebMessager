using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebMessenger.Domains;
using WebMessenger.Models;

namespace WebMessenger.Services
{
    public class MessageService : IMessageService
    {
        private static MessageService _singleTone;
        private IList<MessageDomain> _allMessages;

        private MessageService()
        {
            _allMessages = new List<MessageDomain>();
        }

        public static IMessageService GetInstance()
        {
            if (_singleTone == null)
                _singleTone = new MessageService();
            return _singleTone;
        }

        public void SaveMessage(UserDomain Sender, UserDomain Reciver, string Message)
        {

            lock (new object())
            {
                int id = _allMessages.Count() == 0 ? 1 : _allMessages.Max(x => x.Id) + 1;
                DateTime date = DateTime.Now;
                _allMessages.Add(new MessageDomain() { Id = id, Sender = Sender, Receiver = Reciver, Message = Message, SendDate = date });
            }
        }

        public object GetMessagesByUser(int UserId)
        {
            var allSendPrivateMessage = _allMessages.Where(x => x.Sender.Id == UserId && x.Receiver.Id != 0)
                .GroupBy(x => x.Receiver.Id, (key, messages) => new { Id = key, Messages = messages.ToList() })
                .SelectMany(x => x.Messages.Select(y => new { UserId = x.Id, Sender = y.Sender, Reciver = y.Receiver, SeneDate = y.SendDate, Message = y.Message }))
                .ToList();

            var allRecivePrivateMessage = _allMessages.Where(x => x.Receiver.Id == UserId && x.Sender.Id != 0)
                .GroupBy(x => x.Sender.Id, (key, messages) => new { Id = key, Messages = messages.ToList() })
                .SelectMany(x => x.Messages.Select(y => new { UserId = x.Id, Sender = y.Sender, Reciver = y.Receiver, SeneDate = y.SendDate, Message = y.Message }))
                .ToList();

            var allPublicMessage = _allMessages.Where(x => x.Receiver.Id == 0)
                .GroupBy(x => x.Receiver.Id, (key, messages) => new { Id = key, Messages = messages.ToList() })
                .SelectMany(x => x.Messages.Select(y => new { UserId = x.Id, Sender = y.Sender, Reciver = y.Receiver, SeneDate = y.SendDate, Message = y.Message }))
                 .ToList();

            var allMessages = allSendPrivateMessage.Union(allRecivePrivateMessage).Union(allPublicMessage)
                .GroupBy(x => x.UserId,
                (key, messages) => new { UserId = key, Messages = messages.Select(x => new MessageModel() { SenderName = x.Sender.Name, ReceiverName = x.Reciver.Name, Message = x.Message, SendDate = x.SeneDate }).OrderBy(y => y.SendDate).ToList() })
                .ToList();

            return allMessages;
        }
    }
}