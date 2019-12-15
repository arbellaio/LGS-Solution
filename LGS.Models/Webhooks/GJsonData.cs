using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LGS.Models.Webhooks
{
    public class WebhookLead
    {
        public string lead_id { get; set; }
        public int campaign_id { get; set; }
        public string gcl_id { get; set; }
        public List<UserLeadColumnData> user_column_data { get; set; }
        public string api_version { get; set; }
        public int form_id { get; set; }
        public string Google_key { get; set; }
        public bool is_test { get; set; }
    }

    public class UserLeadColumnData
    {
        public string column_name { get; set; }
        public string string_value { get; set; }
    }
}