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
    }
}
