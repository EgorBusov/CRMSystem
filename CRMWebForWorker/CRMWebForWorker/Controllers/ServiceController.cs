using System.Net;
using System.Web.Http;
using CRMWebForWorker.ApiInteraction.ApiRequests;
using CRMWebForWorker.Models.ServiceModels;
using Microsoft.AspNetCore.Mvc;

namespace CRMWebForWorker.Controllers
{
    public class ServiceController : Controller
    {
        private readonly ServiceRequests _serviceRequests;
        public ServiceController(ServiceRequests serviceRequests)
        {
            _serviceRequests = serviceRequests;
        }

        /// <summary>
        /// Получение всех услуг
        /// </summary>
        /// <returns></returns>
        [Microsoft.AspNetCore.Mvc.HttpGet]
        public async Task<IActionResult> GetServices()
        {
            try
            {
                IEnumerable<Service> services = await _serviceRequests.GetServicesRequest();
                return View(services);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }

        /// <summary>
        /// Получение информации об услуге по Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Mvc.HttpGet]
        public async Task<IActionResult> GetServiceById(int id)
        {
            try
            {
                Service service = await _serviceRequests.GetServiceByIdRequest(id);
                return View(service);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }

        }

        /// <summary>
        /// Редактирование услуги
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Mvc.HttpGet]
        public async Task<IActionResult> EditService(int id)
        {
            try
            {
                Service service = await _serviceRequests.GetServiceByIdRequest(id);
                return View(service);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }

        /// <summary>
        /// Редактирование услуги
        /// </summary>
        /// <param name="service"></param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Mvc.HttpPut]
        public async Task<IActionResult> EditService([Microsoft.AspNetCore.Mvc.FromBody] Service service)
        {
            try
            {
                string token = Request.Cookies["jwt"] ?? throw new HttpResponseException(HttpStatusCode.Unauthorized);
                await _serviceRequests.EditServiceRequest(service, token);
                return Redirect("/Service/GetServices");
            }
            catch (HttpResponseException ex)
            {
                if (ex.Response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    return Redirect("/Account/Login");
                }
                ModelState.AddModelError("", ex.Message);
                return Redirect("/Service/GetServices");

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return Redirect("/Service/GetServices");
            }
        }

        /// <summary>
        /// Удаление услуги
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Mvc.HttpDelete]
        public async Task<IActionResult> DeleteService(int id)
        {
            try
            {
                string token = Request.Cookies["jwt"] ?? throw new HttpResponseException(HttpStatusCode.Unauthorized);
                await _serviceRequests.DeleteServiceRequest(id, token);
                return Redirect("/Service/GetServices");
            }
            catch (HttpResponseException ex)
            {
                if (ex.Response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    return Redirect("/Account/Login");
                }
                ModelState.AddModelError("", ex.Message);
                return Redirect("/Service/GetServices");

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return Redirect("/Service/GetServices");
            }
        }

        /// <summary>
        /// Добавление услуги
        /// </summary>
        /// <returns></returns>
        [Microsoft.AspNetCore.Mvc.HttpGet]
        public IActionResult AddService()
        {
            return View();
        }

        /// <summary>
        /// Добавление услуги
        /// </summary>
        /// <returns></returns>
        [Microsoft.AspNetCore.Mvc.HttpGet]
        public async Task<IActionResult> AddService([Microsoft.AspNetCore.Mvc.FromBody] Service service)
        {
            try
            {
                string token = Request.Cookies["jwt"] ?? throw new HttpResponseException(HttpStatusCode.Unauthorized);
                await _serviceRequests.AddServiceRequest(service, token);
                return Redirect("/Service/GetServices");
            }
            catch (HttpResponseException ex)
            {
                if (ex.Response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    return Redirect("/Account/Login");
                }
                ModelState.AddModelError("", ex.Message);
                return Redirect("/Service/GetServices");

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return Redirect("/Service/GetServices");
            }
        }
    }
}
