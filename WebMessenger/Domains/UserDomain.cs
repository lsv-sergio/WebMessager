using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebMessenger.Models;

namespace WebMessenger.Domains
{
    public class UserDomain
    {
        private string _connectionId;

        public UserDomain()
        {
            _connectionId = "";
        }

        public int Id { get; set; }
       
        public string Name
        {
            get
            {
                return LoginSettings == null ? "" : LoginSettings.LoginName;
            }
        }
        
        public string ConnectionId
        {
            get
            {
                return _connectionId;
            }
        }
        
        public LoginModel LoginSettings { get; set; }

        public void Login(string newConnetcionId)
        {
            _connectionId = newConnetcionId;
        }

        public void Logout()
        {
            _connectionId = "";
        }

    }
}