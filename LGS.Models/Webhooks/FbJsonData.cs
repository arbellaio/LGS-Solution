using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LGS.Models.Webhooks
{
    public class FbLead
    {
        public FbLeadDetail leadgen_forms { get; set; }
        public string id { get; set; }

    }

    public class FbLeadDetail
    {
        public List<FbLeadData> data { get; set; }
    }


    public class FbFieldData
    {
        public string name { get; set; }
        public List<string> values { get; set; }
    }

    public class Data
    {
        public DateTime created_time { get; set; }
        public string id { get; set; }
        public List<FbFieldData> field_data { get; set; }
    }

   

    public class Leads
    {
        public List<Data> data { get; set; }
    }

    public class FbLeadData
    {
        public Leads leads { get; set; }
        public string id { get; set; }
    }

    public class Cursors2
    {
        public string before { get; set; }
        public string after { get; set; }
    }

    public class Paging2
    {
        public Cursors2 cursors { get; set; }
    }

    public class Cursors3
    {
        public string before { get; set; }
        public string after { get; set; }
    }

    public class Paging3
    {
        public Webhooks.Cursors2 cursors { get; set; }
    }
}
