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

                // Get Date Of Post
                var urlParts = post.Page_Path.Split('/');
                post.DateOfPost = new DateTime(int.Parse(urlParts[2]), int.Parse(urlParts[3]), int.Parse(urlParts[4]));

                // doing something with item
                posts.Add(post);
            }

            // Organize posts by DateOfPost descending and grab just the last 30
            var oPosts = posts.OrderByDescending(o => o.DateOfPost).Take(30).ToList();

            // If successful, purge data and prepare to add new posts
            foreach (var ipost in oPosts)
            {
                var newsItem = new ExternalNewsItem()
                {
                    Audience = ipost.User_Type,
                    BackgroundImageName = ipost.Cloud_file_Name,
                    Body = ipost.Body,
                    Byline = ipost.Byline,
                    ExeternalId = ipost.Content_ID,
                    ExternalLink = ipost.Page_Path,
                    IsArticle = ipost.Is_Article,
                    IsDocument = ipost.Is_Document,
                    IsExternalLink = ipost.Is_External_Link,
                    IsQuiz = ipost.Is_Quiz,
                    IsVideo = ipost.Is_Video,
                    PrimaryTag = ipost.Main_Tag,
                    Title = ipost.Post_Title,
                    DateParsed = DateTime.UtcNow
                };

                // Get Date Of Post
                var urlParts = newsItem.ExternalLink.Split('/');
                newsItem.DateOfPost = ipost.DateOfPost;

                // Get Slug
                var cleanTitle = Regex.Replace(newsItem.Title.Replace(" ", "_"), @"[^A-Za-z0-9_]+", "");
                newsItem.Slug = "/" + urlParts[2] + "/" + urlParts[3] + "/" + urlParts[4] + "/" + cleanTitle;

                // If an existing news item cannot be found, add it. Otherwise update it.
                var existNewsItem = _context.ExternalNewsItems.FirstOrDefault(w => w.ExeternalId == newsItem.ExeternalId);
                if (existNewsItem == null)
                {
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
                }
                else   // Update the news item
                {
                    existNewsItem.Audience = newsItem.Audience;
                    existNewsItem.Body = newsItem.Body;
                    existNewsItem.Byline = newsItem.Byline;
                    existNewsItem.DateExpired = newsItem.DateExpired;
                    existNewsItem.DateOfPost = newsItem.DateOfPost;
                    existNewsItem.ExternalLink = newsItem.ExternalLink;
                    existNewsItem.IsArticle = newsItem.IsArticle;
                    existNewsItem.IsDocument = newsItem.IsDocument;
                    existNewsItem.IsExternalLink = newsItem.IsExternalLink;
                    existNewsItem.IsQuiz = newsItem.IsQuiz;
                    existNewsItem.IsVideo = newsItem.IsVideo;
                    existNewsItem.PrimaryTag = newsItem.PrimaryTag;
                    existNewsItem.Slug = newsItem.Slug;
                    existNewsItem.Title = newsItem.Title;
                }
            } // next item

            // Save Changes
            _context.SaveChanges();

            // Return OK
            return "OK";
        }
    }
}
