using CRMApi.Context;
using CRMApi.Interfaces;
using CRMApi.Models.AccountModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        public string AddUser(RegisterModel model)
        {
            if(model.Password == null || model.UserName == null || model.Email == null) 
            { throw new NullReferenceException("не все обязательные поля заполнены"); }
            if (CheckUserName(model.UserName)) { throw new Exception("Пользователь с таким логином уже существует"); }
            User user = new User() { Email = model.Email, UserName = model.UserName, 
                                    PasswordHash = _jwt.HashPassword(model.Password), Role = "Admin" };
            _context.Users.Add(user);
            _context.SaveChanges();
            return _jwt.GenerateJWT(user);
        }

        public bool CheckUserName(string userName)
        {
            var checkUser = _context.Users.FirstOrDefault(a => a.UserName == userName);
            if (checkUser != null) { return true; }
            return false;
        }

        public User GetUserByLogin(string login)
        {
            return _context.Users.FirstOrDefault(u => u.UserName == login) ?? new User();
        }

        public User GetUserByLogPass(LoginModel model)
        {
            var user = _context.Users.FirstOrDefault(a => a.UserName == model.UserName &&
                                                          a.PasswordHash == _jwt.HashPassword(model.Password));
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
        public void EditPassword(EditPasswordModel edit)
        {
            User user = _context.Users.FirstOrDefault(a => a.Id.Equals(edit.UserId)) ?? throw new Exception("Запись не найдена");
            if (user.PasswordHash != _jwt.HashPassword(edit.OldPassword)) { throw new Exception("Пароль неверный"); }
            user.PasswordHash = _jwt.HashPassword(edit.NewPassword);
            _context.SaveChanges();
        }

        public User GetUserById(int id)
        {
            return _context.Users.FirstOrDefault(a =>a.Id == id) ?? new User();
        }
    }
}
