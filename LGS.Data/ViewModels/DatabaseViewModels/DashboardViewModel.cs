using LGS.Models;
using LGS.Models.Analytics;
using System.Collections.Generic;

namespace LGS.Data.ViewModels.DatabaseViewModels
{
    public class DashboardViewModel
    {
        public int RegisteredClients { get; set; }
        public int RegisteredSubAdmins { get; set; }
        public List<Analytics_Visits> UniqueUsers { get; set; }
        public List<ApplicationUser> Users { get; set; }
    }
}