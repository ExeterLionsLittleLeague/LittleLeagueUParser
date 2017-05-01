using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using LittleLeagueUParser.Core;
using LittleLeagueUParser.Data;

namespace LittleLeagueUParser.Controllers
{
    public class ParseController : Controller
    {
        private readonly LittleLeagueDbContext _context;

        public ParseController(LittleLeagueDbContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> LittleLeagueU()
        {
            var parseContent = new LittleLeagueUContent(_context);
            var results = await parseContent.Parse("http://localhost:21910/js/content.json");

            var rtn = new ContentResult();
            rtn.Content = results;

            return rtn;
        }
    }
}