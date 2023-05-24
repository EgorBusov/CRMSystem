using CRMApi.Interfaces;
using CRMApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRMApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        private readonly IServiceData _serviceData;

        public ServiceController(IServiceData serviceData)
        {
            _serviceData = serviceData;
        }
        /// <summary>
        /// Получение всех услуг
        /// </summary>
        /// <returns></returns>
        [Route("GetServices")]
        [HttpGet]
        [AllowAnonymous]
        public IEnumerable<Service> GetServices()
        {
            return _serviceData.GetServices() ?? new List<Service>();
        }
        /// <summary>
        /// Получение услуги по Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("GetServiceById/{id}")]
        [HttpGet("{id}")]
        [AllowAnonymous]
        public Service GetServiceById(int id)
        {
            try
            {
                return _serviceData.GetServiceById(id);
            }
            catch (Exception ex)
            {
                return new Service();
            }
        }
        /// <summary>
        /// Изменение услуги
        /// </summary>
        /// <param name="service"></param>
        /// <returns></returns>
        [Route("EditService")]
        [HttpPut]
        [Authorize(Roles = "Admin")]
        public IActionResult EditService([FromBody] Service service)
        {
            try
            {
                _serviceData.EditService(service);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Удаление услуги
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("DeleteService/{id}")]
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteService(int id) 
        {
            try
            {
                Service service = _serviceData.GetServiceById(id);
                _serviceData.DeleteService(service);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Добавление услуги
        /// </summary>
        /// <param name="service"></param>
        /// <returns></returns>
        [Route("AddService")]
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult AddService([FromBody] Service service)
        {
            try
            {
                _serviceData.AddService(service);
                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}
