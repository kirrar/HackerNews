using HackerNewsPortal.Contracts;
using HackerNewsPortal.DataContext;
using HackerNewsPortal.Models;
using HackerNewsPortal.Providers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace HackerNewsPortal.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HackerNewsController : ControllerBase
    {
        private readonly IHackerNewsProvider _hackerNewsProvider;
        private readonly Data _data;

        public HackerNewsController(IHackerNewsProvider hackerNewsProvider, Data data)
        {
            _hackerNewsProvider = hackerNewsProvider;
            _data = data;
        }

        [HttpGet, Route("GetAllStoryIds")]
        [ProducesResponseType(typeof(List<int>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult GetAllStoryIds()
        {
            var storyIds = _hackerNewsProvider.GetStoryIds();

            if (storyIds.Count == 0)
            {
                return NoContent();
            }

            return Ok(storyIds);
        }

        [HttpGet, Route("GetPage")]
        [ProducesResponseType(typeof(PaginationResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult GetPage([FromQuery] PaginationRequest pagination)
        {
            var response = new PaginationResponse();

            response.Stories = _data.Stories.Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize).ToList();

            response.TotalStories = _data.Stories.Count();
            response.PageNumber = pagination.PageNumber;
            response.PageSize = pagination.PageSize;

            if (response.Stories.Count == 0)
            {
                return NoContent();
            }

            return Ok(response);
        }

        // need search endpoint by title?

        [HttpGet, Route("GetStory")]
        [ProducesResponseType(typeof(Story), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult GetStory([FromQuery] int storyId)
        {
            var story = _data.Stories.Where(x => x.id == storyId).FirstOrDefault();

            if(string.IsNullOrEmpty(story.title))
            {
                return NoContent();
            }

            return Ok(story);
        }

        [HttpGet, Route("GetStories")]
        [ProducesResponseType(typeof(List<Story>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult GetStories([FromQuery] List<int> storyIds)
        {
            var stories = _hackerNewsProvider.GetStories(storyIds);

            if (stories.Count == 0)
            {
                return NoContent();
            }

            return Ok(stories);
        }

        [HttpGet, Route("SearchStories")]
        [ProducesResponseType(typeof(PaginationResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult SearchStories([FromQuery] SearchPaginationRequest searchPaginationTerm)
        {
            var response = new PaginationResponse();

            response.Stories = _data.Stories.Where(x => x.title.Contains(searchPaginationTerm.SearchTerm))
                .Skip((searchPaginationTerm.PageNumber - 1) * searchPaginationTerm.PageSize)
                .Take(searchPaginationTerm.PageSize).ToList();

            response.TotalStories = response.Stories.Count;
            response.PageNumber = searchPaginationTerm.PageNumber;
            response.PageSize = searchPaginationTerm.PageSize;

            if (response.Stories.Count == 0)
            {
                return NoContent();
            }

            return Ok(response);
        }
    }
}
