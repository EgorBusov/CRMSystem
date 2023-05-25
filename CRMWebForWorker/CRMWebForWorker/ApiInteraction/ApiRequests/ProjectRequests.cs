using CRMWebForWorker.Models.BlogModels;
using CRMWebForWorker.Models.ProjectModels;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Web.Http;

namespace CRMWebForWorker.ApiInteraction.ApiRequests
{
    /// <summary>
    /// Запросы касаемо проектов
    /// </summary>
    public class ProjectRequests
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        public ProjectRequests(HttpClient client, string baseUrl)
        {
            _httpClient = client;
            _baseUrl = baseUrl;
        }
        /// <summary>
        /// Добавление проекта
        /// </summary>
        /// <param name="model"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<bool> AddProjectRequest(ProjectModel model, string token)
        {
            MultipartFormDataContent content = new MultipartFormDataContent();
            content.Add(new StringContent(model.Title), "Title");
            content.Add(new StringContent(model.Description), "Description");

            var pictureContent = new StreamContent(model.Picture.OpenReadStream());
            content.Add(pictureContent, "Picture");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.PostAsync($"{_baseUrl}/Project/AddProject", content);
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
        /// Удаление проекта
        /// </summary>
        /// <param name="id"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        /// <exception cref="HttpResponseException"></exception>
        public async Task<bool> DeleteProjectRequest(int id, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.DeleteAsync($"{_baseUrl}/Project/DeleteProject/{id}");
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
        /// Получение всех проектов
        /// </summary>
        /// <returns></returns>
        /// <exception cref="HttpResponseException"></exception>
        public async Task<IEnumerable<ProjectPath>> GetProjectsRequest()
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/Project/GetProjects");
            if (response.IsSuccessStatusCode)
            {
                var responseStream = await response.Content.ReadAsStreamAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase // имена свойств должны быть в формате camelCase
                };
                var models = await JsonSerializer.DeserializeAsync<IEnumerable<ProjectPath>>(responseStream, options);
                return models ?? new List<ProjectPath>();
            }
            else
            {
                throw new HttpResponseException(response.StatusCode);
            }
        }
        /// <summary>
        /// Изменение проекта
        /// </summary>
        /// <param name="model"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        /// <exception cref="HttpResponseException"></exception>
        public async Task<bool> EditProjectRequest(ProjectModel model, string token)
        {
            MultipartFormDataContent content = new MultipartFormDataContent();
            content.Add(new StringContent(Convert.ToString(model.Id)), "Id");
            content.Add(new StringContent(model.Title), "Title");
            content.Add(new StringContent(model.Description), "Description");

            if (model.Picture.Length > 0)
            {
                var pictureContent = new StreamContent(model.Picture.OpenReadStream());
                content.Add(pictureContent, "Picture");
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.PutAsync($"{_baseUrl}/Project/EditProject", content);
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
        /// Получение проекта по Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="HttpResponseException"></exception>
        public async Task<ProjectPath> GetProjectByIdRequest(int id)
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/Project/GetProjectById/{id}");
            if (response.IsSuccessStatusCode)
            {
                var responseStream = await response.Content.ReadAsStreamAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase // имена свойств должны быть в формате camelCase
                };
                var model = await JsonSerializer.DeserializeAsync<ProjectPath>(responseStream, options);
                return model ?? new ProjectPath();
            }
            else
            {
                throw new HttpResponseException(response.StatusCode);
            }
        }
    }
}
