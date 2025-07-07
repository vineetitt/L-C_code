using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsNotifierClient.Models
{
    public class NewsArticles
    {
        public int ArticleID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string URL { get; set; }
    }
}
