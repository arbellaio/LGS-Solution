using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LGS.Models;
using LGS.Models.Analytics;

namespace LGS.Data.ViewModels.AdminViewModels
{
    public class DashboardViewModel
    {
        public int RegisteredClients { get; set; }
        public int RegisteredSubAdmins { get; set; }
        public List<Analytics_Visits> UniqueUsers { get; set; }
        public List<ApplicationUser> Users { get; set; }
    }
}