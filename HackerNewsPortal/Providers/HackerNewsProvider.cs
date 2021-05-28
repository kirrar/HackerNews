using HackerNewsPortal.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using System.Linq;
using HackerNewsPortal.DataContext;

namespace HackerNewsPortal.Providers
{
    public class HackerNewsProvider : IHackerNewsProvider
    {
        private readonly IConfiguration _config;
        private readonly Data _data;

        private readonly char[] charsToTrim = { '[', ']' };

        public HackerNewsProvider(IConfiguration config, Data data)
        {
            _config = config;
            _data = data;
        }

        public List<int> GetStoryIds()
        {
            var storyIds = _data.Stories.Select(x => x.id).ToList();

            return storyIds;
        }

        public Story GetStory(int storyId)
        {
            var story = _data.Stories.Where(x => x.id == storyId).FirstOrDefault();

            return story;
        }

        public List<Story> GetStories(List<int> storyIds)
        {
            var stories = new List<Story>();

            foreach(var storyId in storyIds)
            {
                var story = GetStory(storyId);

                // we only want stories with a url
                if (story != null && story.url != null && !string.IsNullOrEmpty(story.url.ToString()))
                {
                    stories.Add(story);
                }
            }

            return stories;
        }
    }
}
