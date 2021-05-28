using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using APP = HackerNewsPortal;

namespace UnitTests
{
    public class IntegrationClient
    {
        private readonly HttpClient _httpClient;

        public IntegrationClient()
        {
            var appFactory = new WebApplicationFactory<APP.Startup>();
            _httpClient = appFactory.CreateClient();
        }

        public HttpClient GetClient()
        {
            return _httpClient;
        }

        public StringContent EncodeRequest<T>(T request)
        {
            var content = JsonConvert.SerializeObject(request);

            var httpContent = new StringContent(content, Encoding.UTF8, "application/json");

            return httpContent;
        }
    }
}
