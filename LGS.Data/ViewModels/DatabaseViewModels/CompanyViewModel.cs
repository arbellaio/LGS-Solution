using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using LGS.Models.Companies;

namespace LGS.Data.ViewModels.DatabaseViewModels
{
    public class CompanyViewModel
    {
        public Company Company { get; set; }
        public HttpPostedFileBase CompanyLogoPic { get; set; }
        public UserViewModel UserViewModel { get; set; }

        public bool IsEnable { get; set; }
        public int LeadPerCredit { get; set; }

        public decimal RemainingItems { get; set; }

        public int RadioNotification { get; set; }
        public int RadioInterval { get; set; }
        public int LeadQuantity { get; set; }
        public int CompanyId { get; set; }
        public int ClientId { get; set; }


        


    }
}
