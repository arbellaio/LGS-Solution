using LGS.Models.Companies;
using System.Web;
using LGS.Models.Items;

namespace LGS.Data.ViewModels.DatabaseViewModels
{
    public class CompanyViewModel
    {
        public Company Company { get; set; }
        public HttpPostedFileBase CompanyLogoPic { get; set; }
        public UserViewModel UserViewModel { get; set; }

        public bool IsEnable { get; set; }
        public decimal LeadPerCredit { get; set; }

        public decimal RemainingItems { get; set; }

        public int RadioNotification { get; set; }
        public int RadioInterval { get; set; }
        public int LeadQuantity { get; set; }
        public int CompanyId { get; set; }
        public int ClientId { get; set; }

        public LgsSetting LgsSetting { get; set; }

        public string GoogleAdKey { get; set; }




    }
}
