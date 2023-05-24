using CRMWebForWorker.Models.OrderModels;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Web.Http;

namespace CRMWebForWorker.ApiInteraction.ApiRequests
{
    /// <summary>
    /// Запросы касаемо заявок
    /// </summary>
    public class OrderRequests
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        public OrderRequests(HttpClient client, string baseUrl)
        {
            _httpClient = client;
            _baseUrl = baseUrl;
        }
        /// <summary>
        /// Получение всех заявок
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Order>> GetOrdersRequest(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.GetAsync($"{_baseUrl}/Order/GetOrders");
            if (response.IsSuccessStatusCode)
            {
                var responseStream = await response.Content.ReadAsStreamAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase // имена свойств должны быть в формате camelCase
                };
                var persones = await JsonSerializer.DeserializeAsync<IEnumerable<Order>>(responseStream, options);
                return persones ?? new List<Order>();
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }
            else
            {
                throw new HttpResponseException(response.StatusCode);
            }
        }
        /// <summary>
        /// Добавление заявки
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        /// <exception cref="HttpResponseException"></exception>
        public async Task<bool> AddOrderRequest(Order order)
        {           
            var json = JsonSerializer.Serialize(order);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{_baseUrl}/Order/AddOrder", data);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }
            else
            {
                throw new HttpResponseException(response.StatusCode);
            }
        }
        /// <summary>
        /// Изменение статуса заявки
        /// </summary>
        /// <param name="status"></param>
        /// <param name="orderId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        /// <exception cref="HttpResponseException"></exception>
        public async Task<bool> EditStatusOrderRequest(string status, int orderId, string token)
        {
            var content = new StringContent(String.Empty);  //как заглушка
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.PutAsync($"{_baseUrl}/Order/EditStatusOrder/{status}/{orderId}", content);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }
            else
            {
                throw new HttpResponseException(response.StatusCode);
            }
        }
        /// <summary>
        /// Получение заявки по id
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<Order> GetOrderByIdRequest(int orderId, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.GetAsync($"{_baseUrl}/Order/GetOrderById/{orderId}");
            if (response.IsSuccessStatusCode)
            {
                var responseStream = await response.Content.ReadAsStreamAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase // имена свойств должны быть в формате camelCase
                };
                var order = await JsonSerializer.DeserializeAsync<Order>(responseStream, options);
                return order ?? new Order();
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }
            else
            {
                throw new HttpResponseException(response.StatusCode);
            }
        }
    }
}
