using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using System;
using System.Linq;
using WebMessenger.Domains;
using WebMessenger.Models;
using WebMessenger.Services;

namespace WebMessenger.Hubs
{
    public class WebMessagerHub : Hub
    {
        IMessageService _messageSever;
        IUserService _userService;

        public WebMessagerHub()
        {
            _messageSever = MessageService.GetInstance();
            _userService = UserService.GetInstance();
        }

        public void SendMessage(int SenderId, int ReciverId, string Message)
        {
            UserDomain sender = _userService.GetUserById(SenderId);
            UserDomain reciver = _userService.GetUserById(ReciverId);
            if (sender == null || reciver == null)
                return;
            string messageJson = JsonConvert.SerializeObject(new { Message = Message, SenderName = sender.Name, SendDate = DateTime.Now.ToShortTimeString() });

            if (reciver.Id == 0)
                Clients.All.IncommingMessage(0, messageJson);
            else
            {
                Clients.Client(reciver.ConnectionId).IncommingMessage(sender.Id, messageJson);
                Clients.Caller.IncommingMessage(reciver.Id, messageJson);
            }
            _messageSever.SaveMessage(sender, reciver, Message);
        }

        public void Connect(int UserId)
        {
            var connectionId = Context.ConnectionId;

            UserDomain user = _userService.GetUserById(UserId);

            var userListJson = JsonConvert.SerializeObject(GetAllConnectedUsers());

            var messagesListJson = JsonConvert.SerializeObject(_messageSever.GetMessagesByUser(user.Id));

            Clients.Caller.OnConnected(user.Id, user.Name, userListJson, messagesListJson);

            user.Login(connectionId);

            _userService.UpdateUser(user);

            Clients.AllExcept(connectionId).OnNewUserConnected(user.Id, user.Name);

        }

        public override System.Threading.Tasks.Task OnDisconnected(bool stopCalled)
        {
            var connectionId = Context.ConnectionId;
            UserDomain user = _userService.GetUserByConnectionId(connectionId);
            if (user != null)
            {
                user.Logout();
                _userService.UpdateUser(user);

                Clients.All.OnClientDisconected(user.Id, user.Name);
            }
            return base.OnDisconnected(stopCalled);
        }

        private object GetAllConnectedUsers()
        {
            return _userService.GetAllConnectedUser().Select(x => new { Id = x.Id, Name = x.Name }).ToList();
        }

    }
}