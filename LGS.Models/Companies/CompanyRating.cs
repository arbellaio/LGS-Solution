using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LGS.Models.Users;

namespace LGS.Models.Companies
{
    public class CompanyRating
    {
        public int Id { get; set; }
        public float Rating { get; set; }

        [ForeignKey("CompanyId")]
        public virtual Company Company { get; set; }
        public int CompanyId { get; set; }
     
    }
}
