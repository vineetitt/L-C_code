using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsNotifierClient.Models
{
    public class ReportedArticleDto
    {
        public int ArticleID { get; set; }
        public string Title { get; set; } = string.Empty;
        public int ReportCount { get; set; }
    }
}
