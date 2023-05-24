using CRMWebForWorker.Models.OrderModels;
using CRMWebForWorker.Models.ServiceModels;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Web.Http;

namespace CRMWebForWorker.ApiInteraction.ApiRequests
{
    /// <summary>
    /// Запросы касаемо услуг
    /// </summary>
    public class ServiceRequests
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        public ServiceRequests(HttpClient client, string baseUrl)
        {
            _httpClient = client;
            _baseUrl = baseUrl;
        }
        /// <summary>
        /// Получение всех услуг
        /// </summary>
        /// <returns></returns>
        /// <exception cref="HttpResponseException"></exception>
        public async Task<IEnumerable<Service>> GetServicesRequest()
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/Service/GetServices");
            if(response.IsSuccessStatusCode)
            {
                var responseStream = await response.Content.ReadAsStreamAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase // имена свойств должны быть в формате camelCase
                };
                var services = await JsonSerializer.DeserializeAsync<IEnumerable<Service>>(responseStream, options);
                return services ?? new List<Service>();
            }
            else
            {
                throw new HttpResponseException(response.StatusCode);
            }
        }
        /// <summary>
        /// Получение услуги по Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="HttpResponseException"></exception>
        public async Task<Service> GetServiceByIdRequest(int id)
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/Service/GetServiceById/{id}");
            if(response.IsSuccessStatusCode)
            {
                var responseStream = await response.Content.ReadAsStreamAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase // имена свойств должны быть в формате camelCase
                };
                var service = await JsonSerializer.DeserializeAsync<Service>(responseStream, options);
                return service ?? new Service();
            }
            else
            {
                throw new HttpResponseException(response.StatusCode);
            }
        }
        /// <summary>
        /// Изменение услуги
        /// </summary>
        /// <param name="service"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        /// <exception cref="HttpResponseException"></exception>
        public async Task<bool> EditServiceRequest(Service service, string token)
        {
            var json = JsonSerializer.Serialize(service);
            var content = new StringContent(json, Encoding.UTF8,"application/json");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.PutAsync($"{_baseUrl}/Service/EditService", content);
            if(response.IsSuccessStatusCode)
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
        /// Удаление услуги
        /// </summary>
        /// <param name="id"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        /// <exception cref="HttpResponseException"></exception>
        public async Task<bool> DeleteServiceRequest(int id, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.DeleteAsync($"{_baseUrl}/Service/DeleteService/{id}");
            if(response.IsSuccessStatusCode)
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
        /// Добавление услуги
        /// </summary>
        /// <param name="service"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        /// <exception cref="HttpResponseException"></exception>
        public async Task<bool> AddServiceRequest(Service service, string token)
        {
            var json = JsonSerializer.Serialize(service);
            var content = new StringContent(json,Encoding.UTF8,"application/json");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.PostAsync(json, content);
            if(response.IsSuccessStatusCode)
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
    }
}
