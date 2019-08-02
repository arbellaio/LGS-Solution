using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LGS.Models.BusinessTypes;

namespace LGS.Models.Companies.CompanyTypes
{
    public class CompanyType
    {
        public int Id { get; set; }
        public string OtherType { get; set; }

        [ForeignKey("CompanyId")]
        public virtual Company Company { get; set; }
        public int CompanyId { get; set; }

        public int BusinessTypeId { get; set; }
    }
}
