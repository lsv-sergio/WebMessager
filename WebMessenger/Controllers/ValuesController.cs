using System.Linq;
using System.Web.Http;
using WebMessenger.Domains;
using WebMessenger.Models;
using WebMessenger.Services;
using WebMessenger.Utils;

namespace WebMessenger.Controllers
{
    public class ValuesController : ApiController
    {
        IUserService _usersService;
        IMessageService _messageService;

        public ValuesController()
        {
            _messageService = MessageService.GetInstance();
            _usersService = UserService.GetInstance();
        }
 
        [HttpPost]
        [Route("api/values/login")]
        public HtmlResult Login(LoginModel LoginModel)
        {
            var viewData = new System.Web.Mvc.ViewDataDictionary();
            viewData.Model = LoginModel;
            var Errors = ModelState.Where(x => x.Value.Errors.Count() > 0).Select(x => new {Key = x.Key, Errors = x.Value.Errors}).ToList();
            foreach(var key in Errors)
            {
                foreach(var error in key.Errors)
                viewData.ModelState.AddModelError(key.Key,error.ErrorMessage);
            }
            if (!viewData.ModelState.IsValid)
                return new HtmlResult("Login", viewData);

            UserDomain user = _usersService.GetUserByName(LoginModel.LoginName);
            if (user == null)
            {
                viewData.ModelState.AddModelError("UserExist", "Пользователя с таким именем не существует. Введите другое имя или выполните регистрацию.");
                return new HtmlResult("Login", viewData);
            }
            if (user.ConnectionId.Trim() != "")
            {
                viewData.ModelState.AddModelError("UserAlredyLogin", "Пользователь с таким именем уже выполнил вход. Введите другое имя.");
                return new HtmlResult("Login", viewData);
            }

            var model = new UserModel()
            {
                Id = user.Id,
                Name = user.Name
            };
            return new HtmlResult("Connected", viewData);
        }
        
        private object GetAllConnectedUsers()
        {
            return _usersService.GetAllConnectedUser().Select(x => new { Id = x.Id, Name = x.Name }).ToList();
        }
    }
}
