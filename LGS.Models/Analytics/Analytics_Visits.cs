using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LGS.Models.Analytics
{
    public class Analytics_Visits
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string IpAddress { get; set; }
        public string Website { get; set; }
        public string Location { get; set; }
        public string UserAgent { get; set; }
        public string Referrer { get; set; }
    }
}
