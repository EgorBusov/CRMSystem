using CRMWebForWorker.ApiInteraction.ApiRequests;
using CRMWebForWorker.Models.AccountModels;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;
using System.Web.Http;

namespace CRMWebForWorker.Controllers
{
    public class AccountController : Controller
    {
        private readonly AccountRequests _accountRequests;

        public AccountController(AccountRequests accountRequests)
        {
            _accountRequests = accountRequests;
        }

        /// <summary>
        /// Вход
        /// </summary>
        /// <returns></returns>
        [Microsoft.AspNetCore.Mvc.HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        
        /// <summary>
        /// Вход
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Mvc.Route(nameof(Login))]
        [Microsoft.AspNetCore.Mvc.HttpPost]
        public async Task<IActionResult> Login([Microsoft.AspNetCore.Mvc.FromBody]LoginModel model)
        {
            try
            {
                string token = await _accountRequests.LoginRequest(model);
                Response.Cookies.Append("jwt", token, new CookieOptions //кладем токен в куки
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode
                        .Strict //указывает, что cookie может быть отправлен только на тот же сайт, на котором он был создан
                });
                Response.Cookies.Append("userName", model.UserName, new CookieOptions //кладем userName в куки
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode
                        .Strict //указывает, что cookie может быть отправлен только на тот же сайт, на котором он был создан
                });
                return Redirect("/Order/GetOrders");
            }
            catch (HttpResponseException ex)
            {
                if (ex.Response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    ModelState.AddModelError("", "Пользователь не найден");
                    return View();
                }
                else
                {
                    ModelState.AddModelError("", ex.Message);
                    return View();
                }
            }
            catch (Exception ex) 
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }

        /// <summary>
        /// Регистрация
        /// </summary>
        /// <returns></returns>
        [Microsoft.AspNetCore.Mvc.HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        /// <summary>
        /// Регистрация
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Mvc.Route(nameof(Register))]
        [Microsoft.AspNetCore.Mvc.HttpPost]
        public async Task<IActionResult> Register([Microsoft.AspNetCore.Mvc.FromBody] RegisterModel model)
        {
            try
            {
                string token = Request.Cookies["jwt"] ?? throw new Exception("Авторизуйтесь");
                await _accountRequests.RegisterRequest(model, token);
                ModelState.AddModelError("", "Логин и пароль выслан на эл. почту.");
                return View();
            }
            catch (Exception ex)
            {
                if(ex.Message == "Авторизуйтесь")
                {
                    return Redirect("/Account/Login");
                }
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }

        /// <summary>
        /// Изменение пароля
        /// </summary>
        /// <returns></returns>
        [Microsoft.AspNetCore.Mvc.HttpGet]
        public IActionResult EditPassword()
        {
            return View();
        }

        /// <summary>
        /// Изменение пароля
        /// </summary>
        /// <returns></returns>
        [Microsoft.AspNetCore.Mvc.Route(nameof(EditPassword))]
        [Microsoft.AspNetCore.Mvc.HttpPost]
        public async Task<IActionResult> EditPassword([Microsoft.AspNetCore.Mvc.FromBody] EditPasswordModel model)
        {
            try
            {
                string token = Request.Cookies["jwt"] ?? throw new Exception("Авторизуйтесь");
                await _accountRequests.EditPasswordRequest(model, token);
                Response.Cookies.Delete("jwt");
                return Redirect("/Account/Login");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }

        /// <summary>
        /// Личный кабинет
        /// </summary>
        /// <returns></returns>
        [Microsoft.AspNetCore.Mvc.HttpGet]
        public IActionResult PersonalAccount()
        {
            try
            {
                ViewBag.UserName = Request.Cookies["userName"] ?? throw new Exception("Авторизуйтесь");
                return View();
            }
            catch (Exception ex)
            {
                if (ex.Message == "Авторизуйтесь")
                {
                    return Redirect("/Account/Login");
                }
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }

        /// <summary>
        /// Выход
        /// </summary>
        /// <returns></returns>
        [Microsoft.AspNetCore.Mvc.HttpGet]
        public IActionResult Denied()
        {
            Response.Cookies.Delete("jwt");
            return Redirect("/Account/Login");
        }
    }
}
