using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebMessenger.Domains;
using WebMessenger.Models;

namespace WebMessenger.Services
{
    public interface IMessageService
    {
        //void SaveMessage(int SenderId, int ReciverId, string Message);
        //void SaveMessage(int UserId, string Message);
        void SaveMessage(UserDomain Sender, UserDomain Reciver, string Message);
        object GetMessagesByUser(int UserId);
    }
}
