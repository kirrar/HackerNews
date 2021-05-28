using HackerNewsPortal.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace UnitTests.Tests
{
    [TestClass]
    public class GetRandomStory
    {
        readonly private HttpClient _httpclient;
        readonly private IntegrationClient _integration;

        public GetRandomStory()
        {
            _integration = new IntegrationClient();
            _httpclient = _integration.GetClient();
        }

        [TestMethod]
        public async Task GetRandomStoryTest()
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
        }
    }
}
