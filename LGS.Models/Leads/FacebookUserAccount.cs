using System.Collections.Generic;

namespace LGS.Models.Leads
{
    public class FacebookUserAccount
    {
        public List<FacebookUserAccountDetail> data { get; set; }
        public Paging paging { get; set; }
    }

    public class FacebookUserAccountDetail
    {
        public string access_token { get; set; }
        public string category { get; set; }
        public List<CategoryList> category_list { get; set; }
        public string name { get; set; }
        public string id { get; set; }
        public List<string> tasks { get; set; }
    }

    public class CategoryList
    {
        public string id { get; set; }
        public string name { get; set; }
    }


    public class Cursors
    {
        public string before { get; set; }
        public string after { get; set; }
    }

    public class Paging
    {
        public Cursors cursors { get; set; }
    }
}