using System.Collections.Immutable;
using CRMWebForWorker.Models.OrderModels;
using CRMWebForWorker.Models.ResourceModels;
using System.Net;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Web.Http;

namespace CRMWebForWorker.ApiInteraction.ApiRequests
{
    /// <summary>
    /// Запросы для получения ресурсов
    /// </summary>
    public class ResourceRequests
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        public ResourceRequests(HttpClient client, string baseUrl)
        {
            _httpClient = client;
            _baseUrl = baseUrl;
        }

        #region Phrases

        /// <summary>
        /// Получение фразы для Header
        /// </summary>
        /// <returns></returns>
        /// <exception cref="HttpResponseException"></exception>
        public async Task<Header> GetPhraseRequest()
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/Resource/GetPhrase");
            if (response.IsSuccessStatusCode)
            {
                var responseStream = await response.Content.ReadAsStreamAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };
                var header = await JsonSerializer.DeserializeAsync<Header>(responseStream, options);
                return header ?? new Header();
            }
            else
            {
                throw new HttpResponseException(response.StatusCode);
            }
        }

        #endregion

        #region Buttons
        /// <summary>
        /// Получение кнопок для главного меню
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Button>> GetButtonsRequest()
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/Resource/GetButtons");
            if (response.IsSuccessStatusCode)
            {
                var responseStream = await response.Content.ReadAsStreamAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };
                var buttons = await JsonSerializer.DeserializeAsync<IEnumerable<Button>>(responseStream, options);
                return buttons ?? Enumerable.Empty<Button>();
            }
            else
            {
                throw new HttpResponseException(response.StatusCode);
            }
        }

        /// <summary>
        /// Изменение кнопки
        /// </summary>
        /// <param name="button"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        /// <exception cref="HttpResponseException"></exception>
        public async Task<bool> EditButtonRequest(Button button, string token)
        {
            var json = JsonSerializer.Serialize(button);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.PutAsync($"{_baseUrl}/Resource/EditButton", content);
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

        #endregion

        #region Contacts

        /// <summary>
        /// Получение контактов
        /// </summary>
        /// <returns></returns>
        /// <exception cref="HttpResponseException"></exception>
        public async Task<IEnumerable<ContactPath>> GetContactsRequest()
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/Resource/GetContacts");
            if (response.IsSuccessStatusCode)
            {
                var responseStream = await response.Content.ReadAsStreamAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };

                var contactModels = await JsonSerializer.DeserializeAsync<IEnumerable<ContactPath>>(responseStream,options);
                return contactModels ?? new List<ContactPath>();
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
        /// Добавление контакта
        /// </summary>
        /// <param name="model"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        /// <exception cref="HttpResponseException"></exception>
        public async Task<bool> AddContactRequest(ContactModel model, string token)
        {
            MultipartFormDataContent content = new MultipartFormDataContent();
            content.Add(new StringContent(model.Link), "Link");

            var pictureContent = new StreamContent(model.Picture.OpenReadStream());
            content.Add(pictureContent, "Picture");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.PostAsync($"{_baseUrl}/Resource/AddContact", content);
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
        /// Удаление контакта
        /// </summary>
        /// <param name="id"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        /// <exception cref="HttpResponseException"></exception>
        public async Task<bool> DeleteContactRequest(int id, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.DeleteAsync($"{_baseUrl}/Resource/DeleteContact/{id}");
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
        #endregion

        #region MainPage

        /// <summary>
        /// Получение информации для основной страницы
        /// </summary>
        /// <returns></returns>
        /// <exception cref="HttpResponseException"></exception>
        public async Task<MainPageContent> GetMainPageRequest()
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/Resource/GetMainPage");
            if (response.IsSuccessStatusCode)
            {
                var responseStream = await response.Content.ReadAsStreamAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };

                var mainPage = await JsonSerializer.DeserializeAsync<MainPageContent>(responseStream, options);
                return mainPage ?? new MainPageContent();
            }
            else
            {
                throw new HttpResponseException(response.StatusCode);
            }
        }

        /// <summary>
        /// Изменение информации на главной странице
        /// </summary>
        /// <param name="mainPage"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        /// <exception cref="HttpResponseException"></exception>
        public async Task<bool> EditMainPageRequest(MainPageContent mainPage,  string token)
        {
            var json = JsonSerializer.Serialize(mainPage);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.PutAsync($"{_baseUrl}/Resource/EditMainPage", content);
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
        #endregion

        #region OurInformation

        /// <summary>
        /// Получение информации о нас
        /// </summary>
        /// <returns></returns>
        /// <exception cref="HttpResponseException"></exception>
        public async Task<OurInformationPath> GetInformationRequest()
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/Resource/GetInformation");
            if (response.IsSuccessStatusCode)
            {
                var responseStream = await response.Content.ReadAsStreamAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };

                var information = await JsonSerializer.DeserializeAsync<OurInformationPath>(responseStream, options);
                return information ?? new OurInformationPath();
            }
            else
            {
                throw new HttpResponseException(response.StatusCode);
            }
        }

        /// <summary>
        /// Редактирование информации о нас
        /// </summary>
        /// <param name="model"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        /// <exception cref="HttpResponseException"></exception>
        public async Task<bool> EditInformationModelRequest(OurInformationModel model, string token)
        {
            MultipartFormDataContent content = new MultipartFormDataContent();
            content.Add(new StringContent(Convert.ToString(model.Id)), "Id");
            content.Add(new StringContent(model.Telephone), "Telephone");
            content.Add(new StringContent(model.Address), "Address");
            content.Add(new StringContent(model.Fax), "Fax");

            if (model.Picture.Length > 0)
            {
                var pictureContent = new StreamContent(model.Picture.OpenReadStream());
                content.Add(pictureContent, "Picture");
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.PutAsync($"{_baseUrl}/Resource/EditInformationModel", content);
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
        #endregion

    }
}
