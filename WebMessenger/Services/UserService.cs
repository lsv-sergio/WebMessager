using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebMessenger.Domains;
using WebMessenger.Models;

namespace WebMessenger.Services
{
    public class UserService : IUserService
    {
        private static UserService _singleTone;
        private IList<UserDomain> _allUsers;

        private UserService()
        {
            _allUsers = new List<UserDomain>();
        }

        public static IUserService GetInstance()
        {
            if (_singleTone == null)
                _singleTone = new UserService();
            return _singleTone;
        }

        public IList<UserDomain> GetAllConnectedUser()
        {
            return _allUsers.Where(x => x.ConnectionId.Trim() != "").ToList();
        }

        public UserDomain RegisterUser(LoginModel Login, out string Message)
        {
            UserDomain existUser = GetUserByName(Login.LoginName);
            if (existUser == null)
            {
                UserDomain newUser = AddUser(Login);
                Message = "Ok";
                return newUser;
            }
            Message = "Пользователь с именем - " + Login.LoginName.Trim() + " уже зарегистрирован";
            return null;
        }

        public UserDomain GetUserByName(string Name)
        {
            return GetUserByFilter(x => x.Name.ToLower().Trim().Equals(Name.ToLower().Trim()));
        }

        public UserDomain GetUserById(int Id)
        {
            return GetUserByFilter(x => x.Id == Id);
        }

        public UserDomain GetUserByConnectionId(string ConnectionId)
        {
            return GetUserByFilter(x => x.ConnectionId.ToLower().Trim().Equals(ConnectionId.ToLower().Trim()));
        }

        public void UpdateUser(UserDomain user)
        {
            if (_allUsers.Contains(user))
                _allUsers.Remove(user);
            _allUsers.Add(user);
        }

        private UserDomain GetUserByFilter(Func<UserDomain, bool> Filter)
        {
            return _allUsers.FirstOrDefault(Filter);
        }

        private UserDomain AddUser(LoginModel Login)
        {
            int id = _allUsers.Count() == 0 ? 1 : _allUsers.Max(x => x.Id) + 1;
            UserDomain newUser = new UserDomain()
            {
                Id = id,
                LoginSettings = Login
            };
            _allUsers.Add(newUser);
            return newUser;
        }
    }
}