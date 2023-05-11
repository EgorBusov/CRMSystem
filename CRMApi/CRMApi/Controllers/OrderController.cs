using CRMApi.Interfaces;
using CRMApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRMApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderData _orderData;
        public OrderController(IOrderData orderData)
        {
            _orderData = orderData;
        }
        /// <summary>
        /// Получение всех заявок
        /// </summary>
        /// <returns></returns>
        [Route("GetOrders")]
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IEnumerable<Order> GetOrders()
        {
            return _orderData.GetOrders() ?? new List<Order>(); 
        }
        /// <summary>
        /// Добавление заявки
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        [Route("AddOrder")]
        [HttpPost]
        [AllowAnonymous]
        public IActionResult AddOrder([FromBody] Order order)
        {
            try { _orderData.AddOrder(order); return Ok(); }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }
        /// <summary>
        /// Редактирование заявки
        /// </summary>
        /// <param name="status"></param>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [Route("EditStatusOrder/{status}/{id}")]
        [HttpPut("{status}/{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult EditStatusOrder(string status, int id)
        {
            try { _orderData.EditStatusOrder(status, id); return Ok(); }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }
        /// <summary>
        /// Получение заявки по Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("GetOrderById/{id}")]
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public Order GetOrderById(int id)
        {
            try { return _orderData.GetOrderById(id); }
            catch { return new Order(); }
        }

    }
}
