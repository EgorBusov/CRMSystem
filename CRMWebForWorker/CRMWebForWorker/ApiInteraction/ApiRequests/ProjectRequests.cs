namespace CRMWebForWorker.ApiInteraction.ApiRequests
{
    public class ProjectRequests
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        public ProjectRequests(HttpClient client, string baseUrl)
        {
            _httpClient = client;
            _baseUrl = baseUrl;
        }
    }
}
