using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace WebMessenger.Models
{
    public class UserSettingsModel
    {
        public string CurrentUser { get; set; }
        public string UsersJson { get; set; }
        public string MessagesJson { get; set; }
    }
}