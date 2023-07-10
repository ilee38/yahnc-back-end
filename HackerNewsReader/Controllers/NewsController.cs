using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HackerNewsReader.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : Controller
    {
      private const string HAKER_NEWS_API_ENDPOINT_PREFIX = "https://hacker-news.firebaseio.com/v0";
      private readonly int MAX_STORIES_TO_LOAD = 30;
      private readonly HttpClient _httpClient;

      public NewsController()
      {
        _httpClient = new HttpClient();
		  }

      // GET: api/news
      [HttpGet]
      public async Task<IActionResult> Get()
      {
        try
        {
          // Hacker-news api endpoint for top stories
          string apiUrl = $"{HAKER_NEWS_API_ENDPOINT_PREFIX}/topstories.json?print=pretty";

          HttpResponseMessage storiesIdsResponse = await _httpClient.GetAsync(apiUrl);

          if (storiesIdsResponse.IsSuccessStatusCode)
          {
            // By default, the hacker-news api returns 500 story ids
            var storiesIdsData = await storiesIdsResponse.Content.ReadAsStringAsync();
            
            // Create a list from the string returned by the api endpoint
            List<string> storiesIdsList = storiesIdsData.Trim('[', ']').Split(", ").ToList();
            var responseData = await GetStoriesProperties(storiesIdsList);

            return responseData != null ? Ok(responseData.Value) : NotFound();
			    }
          else
			    {
			      return NotFound(); 
			    }
		    }
        catch(Exception e)
        {
          return BadRequest();
		    }

		  }

      /// <summary>
      /// Gets the details of each story by Id 
      /// </summary>
      /// <param name="storyIdList">The ids of the stories to get</param>
      /// <returns>A list of JSON objects containing the strory details</returns>
      private async Task<JsonResult> GetStoriesProperties(List<string> storyIdList)
      {
        List<JObject> stories = new List<JObject> ();
        
        for (int i = 0; i <= MAX_STORIES_TO_LOAD; i++)
        {
          // Hacker-news api endpoint for story details 
          string apiUrl = $"{HAKER_NEWS_API_ENDPOINT_PREFIX}/item/{storyIdList[i]}.json?print=pretty";

          try
          {
            HttpResponseMessage storyDetailsResponse = await _httpClient.GetAsync(apiUrl);
            if (storyDetailsResponse.IsSuccessStatusCode)
            {
              string story = await storyDetailsResponse.Content.ReadAsStringAsync();
              JObject? storyJson = JsonConvert.DeserializeObject<JObject>(story);

              if (storyJson != null)
              {
                stories.Add(storyJson);
				      }
				    }
			    }
          catch(Exception e)
          {
            return new JsonResult(BadRequest());
			    }
		    } 

        return new JsonResult(stories);
		  }
    }
}