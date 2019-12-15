using LGS.Models;
using LGS.Models.Users;

namespace LGS.Data.ViewModels.DatabaseViewModels
{
    public class UserViewModel
    {
        public ApplicationUser User { get; set; }
        public string RoleName { get; set; }
        public SubAdmin SubAdmin { get; set; }
        public Client Client { get; set; }


        public int RoleId { get; set; }
    }
}
