using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LGS.Models.Items
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SkuCode { get; set; }
        public string Type { get; set; }
        public string Price { get; set; }
        public string Tax { get; set; }
    }
}
