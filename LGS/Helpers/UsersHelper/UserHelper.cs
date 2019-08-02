using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LGS.Data.ViewModels.AdminViewModels;
using LGS.Models;
using LGS.Models.RoleNames;

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