using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using WebMessenger.Domains;
using WebMessenger.Models;
using WebMessenger.Services;

namespace WebMessenger
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        public static List<UserModel> UsersList = new List<UserModel>() { new UserModel() { Id = 0, Name = "Все пользователи" } };

        //public static Dictionary<int, string> AllConnectedUsers = new Dictionary<int, string>();

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            IUserService userService = UserService.GetInstance();

            IList<string> usersName = new List<string>();
            usersName.Add("Sergio");
            usersName.Add("Dimon");
            usersName.Add("Nastya");

            UserDomain newUser;
            newUser = new UserDomain();
            newUser.Id = 0;
            newUser.LoginSettings = new LoginModel() { LoginName = "Все пользователи", LoginPassword = "ыазщлфкупэфэаоиэхфу кэЩхшОИФЖУК И" };
            newUser.Login(Guid.NewGuid().ToString());
            userService.UpdateUser(newUser);

            int id = 1;
            foreach (string userName in usersName)
            {
                newUser = new UserDomain();
                newUser.Id = id++;
                newUser.LoginSettings = new LoginModel() { LoginName = userName, LoginPassword = "ыазщлфкупэфэаоиэхфу кэЩхшОИФЖУК И" };
                userService.UpdateUser(newUser);
            }
        }
    }
}
