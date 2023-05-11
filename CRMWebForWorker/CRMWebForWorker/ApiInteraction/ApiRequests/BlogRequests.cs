namespace CRMWebForWorker.ApiInteraction.ApiRequests
{
    public class BlogRequests
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        public BlogRequests(HttpClient client, string baseUrl)
        {
            _httpClient = client;
            _baseUrl = baseUrl;
        }
    }
}
