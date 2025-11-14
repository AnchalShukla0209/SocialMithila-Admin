using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialMithila.Models
{
    public class MonthlySeriesDto
    {
        public string MonthLabel { get; set; }
        public int UsersCount { get; set; }
        public int BusinessesCount { get; set; }
        public int PostsCount { get; set; }
    }
}