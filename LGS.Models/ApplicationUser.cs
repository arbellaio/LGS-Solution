using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace LGS.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here

            var roles = await manager.GetRolesAsync(Id);

            userIdentity.AddClaim(new Claim("CustomName", FullName));
            if (roles != null && roles.Count > 0)
            {
                userIdentity.AddClaim(new Claim("CustomRole", roles.FirstOrDefault()));
            }
            return userIdentity;
        }
    }
}