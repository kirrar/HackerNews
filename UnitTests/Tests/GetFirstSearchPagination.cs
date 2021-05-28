using HackerNewsPortal.Contracts;
using HackerNewsPortal.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.Tests
{
    [TestClass]
    public class GetFirstSearchPagination
    {
        readonly private HttpClient _httpclient;
        readonly private IntegrationClient _integration;

        public GetFirstSearchPagination()
        {
            _integration = new IntegrationClient();
            _httpclient = _integration.GetClient();
        }

        [TestMethod]
        public async Task GetFirstSearchPaginationTest()
        {
            var random = new Random();
            var response = await _httpclient.GetAsync("/HackerNews/GetAllStoryIds");

            Assert.IsTrue(response.IsSuccessStatusCode);

            var responseString = await response.Content.ReadAsStringAsync();

            var respondData = JsonConvert.DeserializeObject<List<int>>(responseString);

            var randomStoryId = respondData[random.Next(respondData.Count)];

            var randomStoryResponse = await _httpclient.GetAsync("/HackerNews/GetStory?storyId=" + randomStoryId);

            Assert.IsTrue(randomStoryResponse.IsSuccessStatusCode);

            var randomStoryString = await randomStoryResponse.Content.ReadAsStringAsync();

            var randomStoryData = JsonConvert.DeserializeObject<Story>(randomStoryString);

            Assert.IsTrue(randomStoryData.id == randomStoryId);

            var sentence = randomStoryData.title.Split(' ').ToList();

            var randomStoryPaginationResponse = await _httpclient.GetAsync("/HackerNews/SearchStories??PageNumber=1&PageSize=10&SearchTerm=" + sentence[0]);

            Assert.IsTrue(randomStoryPaginationResponse.IsSuccessStatusCode);

            var randomStoryPaginationString = await randomStoryPaginationResponse.Content.ReadAsStringAsync();

            var randomStoryPaginationData = JsonConvert.DeserializeObject<PaginationResponse>(randomStoryPaginationString);

            Assert.IsTrue(randomStoryPaginationData.TotalStories > 0);
        }
    }
}
