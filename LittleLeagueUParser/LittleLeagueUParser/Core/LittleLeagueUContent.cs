using LittleLeagueUParser.Data;
using LittleLeagueUParser.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace LittleLeagueUParser.Core
{
    public class LittleLeagueUContent
    {
        private readonly LittleLeagueDbContext _context;

        public LittleLeagueUContent(LittleLeagueDbContext context)
        {
            _context = context;
        }

        public async Task<string> Parse(string uri)
        {
            // Create Posting collection
            var posts = new List<LLUPost>();

            // Read JSON
            var httpClient = new HttpClient();
            var content = await httpClient.GetStringAsync(uri);

            // Enumerate content
            foreach (KeyValuePair<string, JToken> item in JObject.Parse(content))
            {
                // Convert value to the Posting Object type
                var post = item.Value.ToObject<LLUPost>();

                // doing something with item
                posts.Add(post);
            }

            /// If successful, purge data and prepare to add new posts
            foreach (var ipost in posts)
            {
                var newsItem = new ExternalNewsItem();
                newsItem.Audience = ipost.User_Type;
                newsItem.BackgroundImageName = ipost.Cloud_file_Name;
                newsItem.Body = ipost.Body;
                newsItem.Byline = ipost.Byline;
                newsItem.ExeternalId = ipost.Content_ID;
                newsItem.ExternalLink = ipost.Page_Path;
                newsItem.IsArticle = ipost.Is_Article;
                newsItem.IsDocument = ipost.Is_Document;
                newsItem.IsExternalLink = ipost.Is_External_Link;
                newsItem.IsQuiz = ipost.Is_Quiz;
                newsItem.IsVideo = ipost.Is_Video;
                newsItem.PrimaryTag = ipost.Main_Tag;
                newsItem.Title = ipost.Post_Title;

                _context.ExternalNewsItems.Add(newsItem);
            }
            _context.SaveChanges();

            // Return OK
            return "OK";
        }
    }
}
