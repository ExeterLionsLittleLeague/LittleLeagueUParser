using LittleLeagueUParser.Data;
using LittleLeagueUParser.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LittleLeagueUParser.Core
{
    public class LittleLeagueUContent
    {
        private readonly LittleLeagueDbContext _context;
        private const string lluri = "http://www.littleleagueu.org";
        private const string banneruri = "http://res.cloudinary.com/little-league/image/upload/c_fill,dpr_1.5,g_face,h_400,q_70,w_2000/";
        private const string blockuri = "http://res.cloudinary.com/little-league/image/upload/c_fill,dpr_1.5,g_face,h_600,q_70,w_600/";

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
                newsItem.DateParsed = DateTime.UtcNow;

                // Get Date Of Post
                var urlParts = newsItem.ExternalLink.Split('/');
                newsItem.DateOfPost = new DateTime(int.Parse(urlParts[2]), int.Parse(urlParts[3]), int.Parse(urlParts[4]));

                // Get Slug
                var cleanTitle = Regex.Replace(newsItem.Title.Replace(" ", "_"), @"[^A-Za-z0-9_]+", "");
                newsItem.Slug = "/" + urlParts[2] + "/" + urlParts[3] + "/" + urlParts[4] + "/" + cleanTitle;

                if (!string.IsNullOrEmpty(ipost.Cloud_file_Name))
                {
                    var imgFile = ipost.Cloud_file_Name;

                    // Get banner image
                    byte[] banner = await httpClient.GetByteArrayAsync(banneruri + imgFile);
                    newsItem.BackgroundImageBanner = banner;

                    // Get block image
                    byte[] block = await httpClient.GetByteArrayAsync(blockuri + imgFile);
                    newsItem.BackgroundImageBlock = block;

                    // Get file type
                    var fileParts = imgFile.Split('.');
                    newsItem.BackgroundImageType = fileParts[fileParts.Length - 1];
                }

                _context.ExternalNewsItems.Add(newsItem);
                _context.SaveChanges();
            }

            // Return OK
            return "OK";
        }
    }
}
