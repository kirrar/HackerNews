using HackerNewsPortal.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace UnitTests.Tests
{
    [TestClass]
    public class GetFirstPagination
    {
        readonly private HttpClient _httpclient;
        readonly private IntegrationClient _integration;

        public GetFirstPagination()
        {
            _integration = new IntegrationClient();
            _httpclient = _integration.GetClient();
        }

        [TestMethod]
        public async Task GetFirstPaginationPageTest()
        {
            var response = await _httpclient.GetAsync("/HackerNews/GetPage?PageNumber=1&PageSize=10");

            Assert.IsTrue(response.IsSuccessStatusCode);

            var responseString = await response.Content.ReadAsStringAsync();

            var respondData = JsonConvert.DeserializeObject<PaginationResponse>(responseString);

            Assert.IsTrue(respondData.TotalStories > 0);
        }
    }
}
