using CRMApi.Interfaces;
using CRMApi.Models.AccountModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace CRMApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountData _accountData;
        private readonly IJWT _jwt;
        private readonly ISenderMail _senderMail;
        public AccountController(IAccountData accountData, IJWT jwt, ISenderMail senderMail)
        {
            _accountData = accountData;
            _jwt = jwt;
            _senderMail = senderMail;
        }
        /// <summary>
        /// Вход в систему
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        [Route("Login")]
        [HttpPost]
        [AllowAnonymous]
        public IActionResult Login([FromBody] LoginModel login)
        {           
            var user = _accountData.GetUserByLogPass(login);

            if (user.Id != 0)
            {
                var tokenString = _jwt.GenerateJWT(user);
                return Ok(new { token = tokenString });
            }

            return Unauthorized();
        }
        /// <summary>
        /// Регистрация в системе
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        [Route("Register")]
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Register([FromBody] RegisterModel model)
        {           
            try
            {                
                var tokenString = _accountData.AddUser(model);
                _senderMail.SendMail(model.Email, "Регистрация", $"Логин: {model.UserName}\nПароль: {model.Password}");
                return Ok(new { token = tokenString });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Изменение пароля
        /// </summary>
        /// <param name="edit"></param>
        /// <returns></returns>
        [Route("EditPassword")]
        [Authorize(Roles = "Admin")]
        [HttpPut]
        public IActionResult EditPassword([FromBody] EditPasswordModel edit)
        {
            if (edit.OldPassword == null || edit.NewPassword == null || edit.UserName == null) { return BadRequest("Заполните все поля"); }
            try
            {
                int id = _accountData.EditPassword(edit);
                var user = _accountData.GetUserById(id);
                _senderMail.SendMail(user.Email, "Изменен пароль", $"Логин: {user.UserName}\nПароль: {edit.NewPassword}");
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
