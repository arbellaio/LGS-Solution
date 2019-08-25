using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LGS.Data.ViewModels.AdminViewModels;
namespace LGS.Models.AdminViewModels.DashboardViewModels
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



        #endregion


        #region Client  / Sub-Admin Registration Property

        public RegisterViewModel RegisterVm { get; set; }

        #endregion
    }
}