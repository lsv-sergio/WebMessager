using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebMessenger.Models;
using Newtonsoft.Json;
using WebMessenger.Domains;

namespace WebMessenger.Tests.Controllers
{
    [TestClass]
    public class ValuesControllerTest
    {
        //[TestMethod]
        //public void Get()
        //{
        //    // Упорядочение
        //    ValuesController controller = new ValuesController();

        //    // Действие
        //    IEnumerable<string> result = controller.Get();

        //    // Утверждение
        //    Assert.IsNotNull(result);
        //    Assert.AreEqual(2, result.Count());
        //    Assert.AreEqual("value1", result.ElementAt(0));
        //    Assert.AreEqual("value2", result.ElementAt(1));
        //}

        //[TestMethod]
        //public void GetById()
        //{
        //    // Упорядочение
        //    ValuesController controller = new ValuesController();

        //    // Действие
        //    string result = controller.Get(5);

        //    // Утверждение
        //    Assert.AreEqual("value", result);
        //}

        //[TestMethod]
        //public void Post()
        //{
        //    // Упорядочение
        //    ValuesController controller = new ValuesController();

        //    // Действие
        //    controller.Post("value");

        //    // Утверждение
        //}

        //[TestMethod]
        //public void Put()
        //{
        //    // Упорядочение
        //    ValuesController controller = new ValuesController();

        //    // Действие
        //    controller.Put(5, "value");

        //    // Утверждение
        //}

        //[TestMethod]
        //public void Delete()
        //{
        //    // Упорядочение
        //    ValuesController controller = new ValuesController();

        //    // Действие
        //    controller.Delete(5);

        //    // Утверждение
        //}

        [TestMethod]
        public void TestList()
        {
            UserDomain allUser = new UserDomain() { Id = 0, LoginSettings = new LoginModel() { LoginName = "Все пользователи" } };
            UserDomain sergio = new UserDomain() { Id = 1, LoginSettings = new LoginModel() { LoginName = "Sergio"} };
            UserDomain nastya = new UserDomain() { Id = 2, LoginSettings = new LoginModel() { LoginName = "Nastya" }};
            UserDomain dimon = new UserDomain() { Id = 3, LoginSettings = new LoginModel() { LoginName = "Dimon" } };

            IList<MessageDomain> _allMessages = new List<MessageDomain>();
            _allMessages.Add(new MessageDomain() { Id = 1, Sender = allUser, Receiver = allUser, Message = "К нам присоединился Sergio", SendDate = DateTime.Now.AddMinutes(-40) });
            _allMessages.Add(new MessageDomain() { Id = 2, Sender = allUser, Receiver = allUser, Message = "К нам присоединился Nastya", SendDate = DateTime.Now.AddMinutes(-35) });
            _allMessages.Add(new MessageDomain() { Id = 3, Sender = sergio, Receiver = nastya, Message = "Привет, как дела", SendDate = DateTime.Now.AddMinutes(-30) });
            _allMessages.Add(new MessageDomain() { Id = 4, Sender = nastya, Receiver = sergio, Message = "Привет, нормально", SendDate = DateTime.Now.AddMinutes(-25) });
            _allMessages.Add(new MessageDomain() { Id = 5, Sender = allUser, Receiver = allUser, Message = "К нам присоединился Dimon", SendDate = DateTime.Now.AddMinutes(-20) });
            _allMessages.Add(new MessageDomain() { Id = 6, Sender = dimon, Receiver = allUser, Message = "Привет всем", SendDate = DateTime.Now.AddMinutes(-15) });
            _allMessages.Add(new MessageDomain() { Id = 7, Sender = sergio, Receiver = allUser, Message = "Привет, Dimon", SendDate = DateTime.Now.AddMinutes(-10) });
            _allMessages.Add(new MessageDomain() { Id = 8, Sender = nastya, Receiver = allUser, Message = "Привет, Dimon", SendDate = DateTime.Now.AddMinutes(-5) });


            var allSendMessage = _allMessages.Where(x => x.Sender.Id == 2)
                .GroupBy(x => x.Receiver.Id, (key, messages) => new { Id = key, Messages = messages.ToList() })
                .SelectMany(x => x.Messages.Select(y => new { UserId = x.Id, Sender = y.Sender, Reciver = y.Receiver, SeneDate = y.SendDate, Message = y.Message }))
                .ToList();

            var allReciveMessage = _allMessages.Where(x => x.Receiver.Id == 2 || (x.Receiver.Id == 0 && x.Sender.Id == 0))
                .GroupBy(x => x.Sender.Id, (key, messages) => new { Id = key, Messages = messages.ToList() })
                .SelectMany(x => x.Messages.Select(y => new { UserId = x.Id, Sender = y.Sender, Reciver = y.Receiver, SeneDate = y.SendDate, Message = y.Message }))
                .ToList();

            var allMessages = allSendMessage.Union(allReciveMessage)
                .GroupBy(x => x.UserId, 
                (key, messages) => new { UserId = key, Messages = messages.Select(x => new { SenderName = x.Sender.Name, ReciverName = x.Reciver.Name, Message = x.Message, SendDate = x.SeneDate }).OrderBy(y => y.SendDate).ToList() })
                .ToList();

            var messagesJson = JsonConvert.SerializeObject(allMessages);

        }
    }
}
