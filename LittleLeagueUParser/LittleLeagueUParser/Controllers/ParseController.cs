using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using LittleLeagueUParser.Core;

namespace LittleLeagueUParser.Controllers
{
    public class ParseController : Controller
    {
        public async Task<IActionResult> LittleLeagueU()
        {
            var parseContent = new LittleLeagueUContent();
            var results = await parseContent.Parse("http://localhost:21910/js/content.json");

            var rtn = new ContentResult();
            rtn.Content = results;

            return rtn;
        }
    }
}