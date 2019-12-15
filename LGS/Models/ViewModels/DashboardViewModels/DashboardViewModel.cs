using LGS.Data.ViewModels.DatabaseViewModels;
using LGS.Models.Communication;
using LGS.Models.Companies;
using LGS.Models.Users;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using LGS.Models.Items;
using Item = LGS.Models.PaypalItem.Item;

namespace LGS.Models.ViewModels.DashboardViewModels
{
    public class DashboardViewModel
    {
        #region Dashboard Home Page Properties

        public int RegisteredClients { get; set; }
        public int RegisteredSubAdmins { get; set; }
        public int UniqueUsers { get; set; }


        #endregion


        #region Sub-Admin / Client List Property

        public List<UserViewModel> UserViewModels { get; set; }



        #endregion

        public UserViewModel UserVm { get; set; }

        #region Miscellaneous

        public bool IsEnable { get; set; }
        public int SubAdminUserId { get; set; }
        public int ClientUserId { get; set; }
        public HttpPostedFileBase ProfilePic { get; set; }

        public LgsSetting LgsSetting { get; set; }
        #endregion


        

        #region Client  / Sub-Admin Registration Property

        public RegisterViewModel RegisterVm { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        #endregion


        #region Paypal Item Display ViewModel

        public List<Item> Items { get; set; }
        public decimal TotalPrice { get; set; }
        public int LeadCount { get; set; }
        #endregion
    }

    public class CompanyViewModel
    {
        public IEnumerable<Company> Companies { get; set; }
        public Company Company { get; set; }
        public Customer Customer { get; set; }
        public CustomerMessage CustomerMessage { get; set; }
        public CustomerReview CustomerReview { get; set; }
        public int CompanyId { get; set; }
    }

    
}