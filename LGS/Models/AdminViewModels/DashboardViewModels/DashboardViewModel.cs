using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LGS.Data.ViewModels.AdminViewModels;

namespace LGS.Models.AdminViewModels.DashboardViewModels
{
    public class DashboardViewModel
    {
        public int RegisteredClients { get; set; }
        public int RegisteredSubAdmins { get; set; }
        public int UniqueUsers { get; set; }
        public List<UserViewModel> UserViewModels { get; set; }

        #region Client Registration

        public RegisterViewModel RegisterVm { get; set; }
        

        #endregion
    }
}