using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsNotifierClient.Models
{
    public class SavedArticle
    {
        public int ArticleID { get; set; }
        public string Title { get; set; } = string.Empty;
        public DateTime SavedDate { get; set; }
    }
}
