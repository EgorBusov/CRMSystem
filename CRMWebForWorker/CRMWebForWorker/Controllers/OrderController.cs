using System.Net;
using System.Web.Http;
using CRMWebForWorker.ApiInteraction.ApiRequests;
using CRMWebForWorker.Models.OrderModels;
using Microsoft.AspNetCore.Mvc;

namespace CRMWebForWorker.Controllers
{
    public class OrderController : Controller
    {
        private readonly OrderRequests _orderRequests;

        public OrderController(OrderRequests orderRequests)
        {
            _orderRequests = orderRequests;
        }
        /// <summary>
        /// Все заявки
        /// </summary>
        /// <returns></returns>
        [Microsoft.AspNetCore.Mvc.HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            try
            {
                string token = Request.Cookies["jwt"] ?? throw new HttpResponseException(HttpStatusCode.Unauthorized);
                IEnumerable<Order> orders = await _orderRequests.GetOrdersRequest(token);
                return View(orders);
            }
            catch (HttpResponseException ex)
            {
                if (ex.Response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    return Redirect("/Account/Login");
                }
                ModelState.AddModelError("", ex.Message);
                return View();
                
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }

        }

        /// <summary>
        /// Добавление заявки
        /// </summary>
        /// <returns></returns>
        [Microsoft.AspNetCore.Mvc.HttpGet]
        public IActionResult AddOrder()
        {
            return View();
        }
        /// <summary>
        /// Добавление заявки
        /// </summary>
        /// <returns></returns>
        [System.Web.Http.Route(nameof(AddOrder))]
        [Microsoft.AspNetCore.Mvc.HttpPost]
        public async Task<IActionResult> AddOrder([Microsoft.AspNetCore.Mvc.FromBody] Order order)
        {
            try
            {
                await _orderRequests.AddOrderRequest(order);
                return Redirect("/Order/GetOrders");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }

        /// <summary>
        /// Изменение статуса заявки
        /// </summary>
        /// <param name="status"></param>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [System.Web.Http.Route(nameof(EditStatusOrder))]
        [Microsoft.AspNetCore.Mvc.HttpPut]
        public async Task<IActionResult> EditStatusOrder(string status, int orderId)
        {
            try
            {
                string token = Request.Cookies["jwt"] ?? throw new HttpResponseException(HttpStatusCode.Unauthorized);
                await _orderRequests.EditStatusOrderRequest(status, orderId, token);
                return Redirect("/Order/GetOrders");
            }
            catch (HttpResponseException ex)
            {
                if (ex.Response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    return Redirect("/Account/Login");
                }
                ModelState.AddModelError("", ex.Message);
                return Redirect("/Order/GetOrders");

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return Redirect("/Order/GetOrders");
            }
        }

        [Microsoft.AspNetCore.Mvc.HttpGet]
        public async Task<IActionResult> GetOrderById(int id)
        {
            try
            {
                string token = Request.Cookies["jwt"] ?? throw new HttpResponseException(HttpStatusCode.Unauthorized);
                Order order = await _orderRequests.GetOrderByIdRequest(id, token);
                return View(order);
            }
            catch (HttpResponseException ex)
            {
                if (ex.Response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    return Redirect("/Account/Login");
                }
                ModelState.AddModelError("", ex.Message);
                return Redirect("/Order/GetOrders");

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return Redirect("/Order/GetOrders");
            }
        }
    }
}
