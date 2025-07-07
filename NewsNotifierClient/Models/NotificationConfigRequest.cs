using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsNotifierClient.Models
{
    public class NotificationConfigRequest
    {
        public int CategoryID { get; set; }
        public string? Keywords { get; set; }
        public bool IsEnabled { get; set; }
    }
}
