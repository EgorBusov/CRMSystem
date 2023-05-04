using CRMApi.Context;
using CRMApi.Interfaces;
using CRMApi.Models;

namespace CRMApi.Services.Data
{
    public class AccountData : IAccountData
    {
        private readonly CRMSystemContext _context;
        private readonly IJWT _jwt;
        public AccountData(CRMSystemContext context, IJWT jwt)
        {
            _context = context;
            _jwt = jwt;
        }
        public void AddUser(User user)
        {
            if(user.PasswordHash == null || user.UserName == null) { throw new NullReferenceException("не все обязательные поля заполнены"); }
            if (CheckUserName(user)) { throw new Exception("Пользователь с таким логином уже существует"); }
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public bool CheckUserName(User user)
        {
            var checkUser = _context.Users.FirstOrDefault(a => a.UserName == user.UserName);
            if (checkUser != null) { return true; }
            return false;
        }

        public User GetUserByLogin(string login)
        {
            return _context.Users.FirstOrDefault(u => u.UserName == login) ?? new User();
        }

        public User GetUserByLogPass(User e)
        {
            var user = _context.Users.FirstOrDefault(a => a.UserName == e.UserName &&
                                                          a.PasswordHash == _jwt.HashPassword(e.PasswordHash));
            return user ?? new User();
        }

        public IEnumerable<User> GetUsers()
        {
            return _context.Users.ToList() ?? new List<User>();
        }

        public void RemoveUser(int userId)
        {
            User user = _context.Users.FirstOrDefault(a => a.Id == userId);
            if (user == null) { throw new Exception("User не найден"); }
            _context.Users.Remove(user);
            _context.SaveChanges();
        }
    }
}
