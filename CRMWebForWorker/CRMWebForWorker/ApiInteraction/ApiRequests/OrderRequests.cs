namespace CRMWebForWorker.ApiInteraction.ApiRequests
{
    public class OrderRequests
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        public OrderRequests(HttpClient client, string baseUrl)
        {
            _httpClient = client;
            _baseUrl = baseUrl;
        }
    }
}
