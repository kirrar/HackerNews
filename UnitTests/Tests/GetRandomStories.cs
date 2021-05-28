using HackerNewsPortal.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace UnitTests.Tests
{
    [TestClass]
    public class GetRandomStories
    {
        readonly private HttpClient _httpclient;
        readonly private IntegrationClient _integration;

        public GetRandomStories()
        {
            _integration = new IntegrationClient();
            _httpclient = _integration.GetClient();
        }

        [TestMethod]
        public async Task GetRandomStoriesTest()
        {
            var random1 = new Random();
            var random2 = new Random();
            var random3 = new Random();

            var storyIds = new List<int>();

            var response = await _httpclient.GetAsync("/HackerNews/GetAllStoryIds");

            Assert.IsTrue(response.IsSuccessStatusCode);

            var responseString = await response.Content.ReadAsStringAsync();

            var respondData = JsonConvert.DeserializeObject<List<int>>(responseString);

            var randomStoryId1 = respondData[random1.Next(respondData.Count)];
            var randomStoryId2 = respondData[random2.Next(respondData.Count)];
            var randomStoryId3 = respondData[random3.Next(respondData.Count)];

            var parameter = string.Format("storyIds={0}&storyIds={1}&storyIds={2}", randomStoryId1, randomStoryId2, randomStoryId3);

            var randomStoryResponse = await _httpclient.GetAsync(@"/HackerNews/GetStories?" + parameter);

            Assert.IsTrue(randomStoryResponse.IsSuccessStatusCode);

            var randomStoryString = await randomStoryResponse.Content.ReadAsStringAsync();

            var randomStoryData = JsonConvert.DeserializeObject<List<Story>>(randomStoryString);

            var truthCheck = new List<bool>();

            var check1 = randomStoryData.Any(x => x.id == randomStoryId1);
            var check2 = randomStoryData.Any(x => x.id == randomStoryId2);
            var check3 = randomStoryData.Any(x => x.id == randomStoryId3);

            Assert.IsTrue(check1 == check2 == check3 == true);
        }
    }
}
