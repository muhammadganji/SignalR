using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using WebAppSignalR.Contexts;
using WebAppSignalR.Models.Entities;

namespace WebAppSignalR.Models.Services
{
    public interface IUserService
    {
        public User Exist(User user);
    }

    
    public class UserService: IUserService
    {
        private readonly DatabaseContext _db;
        public UserService(DatabaseContext db)
        {
            _db = db;
        }

        public User Exist(User user)
        {
            var findUser = _db.Users.SingleOrDefault(u => u.Username == user.Username && u.Password == user.Password);
            if(findUser != null)
            {
                return findUser;
            }
            return null;
        }
    }
}
