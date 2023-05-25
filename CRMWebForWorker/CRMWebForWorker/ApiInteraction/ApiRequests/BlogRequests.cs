using CRMWebForWorker.Models.BlogModels;
using System.Net;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text.Json;
using System.Web.Http;

namespace CRMWebForWorker.ApiInteraction.ApiRequests
{
    /// <summary>
    /// Запросы касаемо Блога
    /// </summary>
    public class BlogRequests
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        public BlogRequests(HttpClient client, string baseUrl)
        {
            _httpClient = client;
            _baseUrl = baseUrl;
        }
        /// <summary>
        /// Добавление блога
        /// </summary>
        /// <param name="model"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        /// <exception cref="HttpResponseException"></exception>
        public async Task<bool> AddBlogRequest(BlogModel model, string token)
        {
            MultipartFormDataContent content = new MultipartFormDataContent();
            content.Add(new StringContent(model.Title), "Title");
            content.Add(new StringContent(model.Description), "Description");

            var pictureContent = new StreamContent(model.Picture.OpenReadStream());
            content.Add(pictureContent, "Picture");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.PostAsync($"{_baseUrl}/Blog/AddBlog", content);
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
        /// Удаление блога
        /// </summary>
        /// <param name="id"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<bool> DeleteBlogRequest(int id, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.DeleteAsync($"{_baseUrl}/Blog/DeleteBlog/{id}");
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
        /// Получение всех блогов
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<BlogPath>> GetBlogsRequest()
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/Blog/GetBlogs");
            if (response.IsSuccessStatusCode)
            {
                var responeStream = await response.Content.ReadAsStreamAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase // имена свойств должны быть в формате camelCase
                };
                var models = await JsonSerializer.DeserializeAsync<IEnumerable<BlogPath>>(responeStream, options);
                return models ?? new List<BlogPath>();
            }
            else
            {
                throw new HttpResponseException(response.StatusCode);
            }
        }
        /// <summary>
        /// Изменение блога
        /// </summary>
        /// <param name="blogModel"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<bool> EditBlogRequest(BlogModel model, string token)
        {
            MultipartFormDataContent content = new MultipartFormDataContent();
            content.Add(new StringContent(Convert.ToString(model.Id)), "Id");
            content.Add(new StringContent(model.Title), "Title");
            content.Add(new StringContent(model.Description), "Description");

            var pictureContent = new StreamContent(model.Picture.OpenReadStream());
            content.Add(pictureContent, "Picture");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.PutAsync($"{_baseUrl}/Blog/EditBlog", content);
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
        /// Получение блога по Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="HttpResponseException"></exception>
        public async Task<BlogPath> GetBlogByIdRequest(int id)
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/Blog/GetBlogById/{id}");
            if (response.IsSuccessStatusCode)
            {
                var responeStream = await response.Content.ReadAsStreamAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase // имена свойств должны быть в формате camelCase
                };
                var model = await JsonSerializer.DeserializeAsync<BlogPath>(responeStream, options);
                return model ?? new BlogPath();
            }
            else
            {
                throw new HttpResponseException(response.StatusCode);
            }
        }
    }
}
