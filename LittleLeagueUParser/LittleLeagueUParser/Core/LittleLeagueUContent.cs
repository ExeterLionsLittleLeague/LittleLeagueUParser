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
            // Read JSON
            var httpClient = new HttpClient();
            var content = await httpClient.GetStringAsync(uri);

            // Parse into Dynamic
            dynamic p = JObject.Parse(content);

            foreach (KeyValuePair<string, JToken> item in JObject.Parse(content))
            {
                // doing something with item
                var x = "";
            }

            // Return OK
            return "OK";
        }
    }
}
