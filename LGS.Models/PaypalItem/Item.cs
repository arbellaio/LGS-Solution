using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LGS.Models.PaypalItem
{
    public class Item
    {
        public int id { get; set; }
        public string name { get; set; }
        public string currency { get; set; }
        public string price { get; set; }
        public string quantity { get; set; }
        public string sku { get; set; }
        public string description { get; set; }
        public string tax { get; set; }
        public string url { get; set; }
        public string priceperitem { get; set; }
        public string UserId { get; set; }
        public int ClientId { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime UpdatedDateTime { get; set; }
        public string Email { get; set; }
    }
}
