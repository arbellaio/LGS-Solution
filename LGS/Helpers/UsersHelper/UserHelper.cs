using LGS.Data.ViewModels.DatabaseViewModels;
using System.Collections.Generic;
using System.Linq;

namespace LGS.Helpers.UsersHelper
{
    public class UserHelper
    {
        public static List<UserViewModel> GetUsersWithSpecificRole(List<UserViewModel> users, string roleName)
        {
            var usersWithRole = users.Where(x => x.RoleName.Equals(roleName)).ToList();
            return usersWithRole;
        }
    }
}