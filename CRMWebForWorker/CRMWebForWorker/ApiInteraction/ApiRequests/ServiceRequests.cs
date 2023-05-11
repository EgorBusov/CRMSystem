namespace CRMWebForWorker.ApiInteraction.ApiRequests
{
    public class ServiceRequests
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        public ServiceRequests(HttpClient client, string baseUrl)
        {
            _httpClient = client;
            _baseUrl = baseUrl;
        }
    }
}
