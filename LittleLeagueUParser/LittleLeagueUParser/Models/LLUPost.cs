using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LittleLeagueUParser.Models
{
    public class LLUPost
    {
        public int ID { get; set; }
        public string Content_ID { get; set; }
        public string Cloud_file_Name { get; set; }
        public string Cloud_Name { get; set; }
        public string Post_Title { get; set; }
        public string User_Type { get; set; }
        public string Page_Path { get; set; }
        public string Main_Tag { get; set; }
        public string Body { get; set; }
        public string Byline { get; set; }
        public bool Is_Quiz { get; set; }
        public bool Is_Article { get; set; }
        public bool Is_Document { get; set; }
        public bool Is_External_Link { get; set; }
        public bool Is_Video { get; set; }

        //TODO: Location, Tags
    }
}
