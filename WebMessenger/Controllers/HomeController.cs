using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebMessenger.Domains;
using WebMessenger.Models;
using WebMessenger.Services;

namespace WebMessenger.Controllers
{
    public class HomeController : Controller
    {
        
        IUserService _usersService;
        IMessageService _messageService;

        public HomeController()
        {
            _messageService = MessageService.GetInstance();
            _usersService = UserService.GetInstance();
            ViewBag.IsDebugMode = false;

#if DEBUG
            ViewBag.IsDebugMode = true;
#endif
        }

        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";
            return View(new LoginModel());
        }

        [HttpPost]
        public ActionResult Index(LoginModel LoginModel)
        {
            if (!ModelState.IsValid)
                return View(LoginModel);
            UserDomain user = _usersService.GetUserByName(LoginModel.LoginName); 
            if (user == null)
            {
                ModelState.AddModelError("UserExist", "Пользователя с таким именем не существует. Введите другое имя или выполните регистрацию.");
                return View(LoginModel);
            }
            if (user.ConnectionId.Trim() != "")
            { 
                 ModelState.AddModelError("UserAlredyLogin", "Пользователь с таким именем уже выполнил вход. Введите другое имя.");
                return View(LoginModel);
           }

            var model = new UserModel() 
            { 
                Id = user.Id, 
                Name = user.Name
                //,
                //UsersJson = JsonConvert.SerializeObject(GetAllConnectedUsers()),
                //MessagesJson = JsonConvert.SerializeObject(_messageService.GetMessagesByUser(user.Id))
            };
            return View("Connected", model);
        }

    }
}
