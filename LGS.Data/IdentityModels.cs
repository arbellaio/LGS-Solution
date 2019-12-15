using LGS.Models;
using LGS.Models.BusinessTypes;
using LGS.Models.Communication;
using LGS.Models.Companies;
using LGS.Models.Companies.CompanyTypes;
using LGS.Models.Credits;
using LGS.Models.Leads;
using LGS.Models.Users;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using LGS.Models.Items;
using Item = LGS.Models.Items.Item;

namespace LGS.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<BusinessType> BusinessTypes { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<CompanyType> CompanyTypes { get; set; }
        public DbSet<AccountCredit> AccountCredits { get; set; }
        public DbSet<CreditInvoice> CreditInvoices { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<SubAdmin> SubAdmins { get; set; }
        public DbSet<CompanyRating> CompanyRatings { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<CompanyInvoice> CompanyInvoices { get; set; }
        public DbSet<CompanyInventory> CompanyInventories { get; set; }
        public DbSet<CustomerMessage> CustomerMessages { get; set; }
        public DbSet<CustomerReview> CustomerReviews { get; set; }

        public DbSet<GoogleLead> GoogleLeads { get; set; }
        public DbSet<GoogleLeadDetail> GoogleLeadDetails { get; set; }
        public DbSet<LgsSetting> LgsSettings { get; set; }
        public DbSet<FacebookLead> FacebookLeads { get; set; }
        public DbSet<FacebookLeadDetail> FacebookLeadsDetails { get; set; }
        public DbSet<CompanyGoogleKey> CompanyGoogleKeys { get; set; }


        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            Configuration.ProxyCreationEnabled = false;
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}
