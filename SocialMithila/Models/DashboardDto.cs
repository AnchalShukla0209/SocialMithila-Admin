using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialMithila.Models
{
    public class DashboardDto
    {
        public int TotalUsers { get; set; }
        public int TotalCategories { get; set; }
        public int TotalSubCategories { get; set; }
        public int TotalBusinesses { get; set; }
        public int TotalStories { get; set; }
        public int TotalPosts { get; set; }
    }
}