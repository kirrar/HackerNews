using HackerNewsPortal.Models;
using System.Collections.Generic;

namespace HackerNewsPortal.Providers
{
    public interface IHackerNewsProvider
    {
        List<int> GetStoryIds();

        Story GetStory(int storyId);

        List<Story> GetStories(List<int> storyIds);
    }
}
