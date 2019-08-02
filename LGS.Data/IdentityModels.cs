using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using LGS.Models;
using LGS.Models.Analytics;
using LGS.Models.BusinessTypes;
using LGS.Models.Companies;
using LGS.Models.Companies.CompanyTypes;
using LGS.Models.Credits;
using LGS.Models.Users;
using Microsoft.AspNet.Identity.EntityFramework;

namespace LGS.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<BusinessType> BusinessTypes { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<CompanyType> CompanyTypes { get; set; }
        public DbSet<AccountCredit> AccountCredits { get; set; }
        public DbSet<CompanyCredit> CompanyCredits { get; set; }
        public DbSet<CreditInvoice> CreditInvoices { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<SubAdmin> SubAdmins { get; set; }


        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}
