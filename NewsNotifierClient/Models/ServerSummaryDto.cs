using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsNotifierClient.Models
{
    public class ServerSummaryDto
    {
        public int ServerId { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
    }
}
