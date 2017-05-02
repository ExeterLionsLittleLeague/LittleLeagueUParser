using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LittleLeagueUParser.Models
{
    public class ExternalNewsItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ExeternalNewsItemID { get; set; }
        public string ExeternalId { get; set; }
        public string BackgroundImageName { get; set; }
        public string Title { get; set; }
        public string Audience { get; set; }
        public string ExternalLink { get; set; }
        public string PrimaryTag { get; set; }
        public string Body { get; set; }
        public string Byline { get; set; }
        public bool IsQuiz { get; set; }
        public bool IsArticle { get; set; }
        public bool IsDocument { get; set; }
        public bool IsExternalLink { get; set; }
        public bool IsVideo { get; set; }
        public string Slug { get; set; }
        public DateTime DateOfPost { get; set; }
        public DateTime DateParsed { get; set; }
        public DateTime DateExpired { get; set; }
    }
}
