using System.Collections.Generic;
using WebMessenger.Domains;
using WebMessenger.Models;

namespace WebMessenger.Services
{
    public interface IUserService
    {
        IList<UserDomain> GetAllConnectedUser();

        UserDomain RegisterUser(LoginModel Login, out string Message);

        UserDomain GetUserByName(string Name);

        UserDomain GetUserById(int Id);

        UserDomain GetUserByConnectionId(string ConnectionId);

        void UpdateUser(UserDomain user);
    }
}
