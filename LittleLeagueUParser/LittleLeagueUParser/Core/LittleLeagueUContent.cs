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


            // Return OK
            return "OK";
        }
    }
}
