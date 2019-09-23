using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LGS.Data.ViewModels.DatabaseViewModels;
using LGS.Models.Companies;
using LGS.Models.RoleNames;
using LGS.Models.Users;

namespace LGS.Data.Services.ClientServices
{
    public class ClientService : IClientService
    {
        #region AppDbContext
        private readonly ApplicationDbContext _context;
        public ClientService(ApplicationDbContext context)
        {
            _context = context ?? throw new NullReferenceException("Context");
        }
        #endregion

        #region Client Crud Methods
        public async Task<UserViewModel> GetClientUserById(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                using (var context = new ApplicationDbContext())
                {
                    var clientInDbWithCompaniesCredits = await context.Clients.Include(x => x.User).Include(x => x.Companies).Include(x => x.AccountCredits).Include(x => x.CreditInvoices).FirstOrDefaultAsync(x => x.AppUserId.Equals(id));
                    var userVm = new UserViewModel
                    {
                        Client = clientInDbWithCompaniesCredits,
                        User = clientInDbWithCompaniesCredits?.User
                    };
                    return userVm;
                }
            }
            return null;
        }

        public async Task<bool> UpdateClientAppUser(UserViewModel userViewModel)
        {
            if (userViewModel?.Client != null)
            {
                var clientAppUser = await GetClientUserById(userViewModel.Client.AppUserId);
                if (clientAppUser != null)
                {
                    clientAppUser.Client.UpdatedDate = DateTime.UtcNow;
                    clientAppUser.Client.ProfilePhoto = userViewModel.Client.ProfilePhoto;
                    clientAppUser.User.FullName = userViewModel.User.FullName;
                    clientAppUser.User.Email = userViewModel.User.Email;
                    _context.Users.AddOrUpdate(clientAppUser.User);
                    _context.Clients.AddOrUpdate(clientAppUser.Client);
                    await _context.SaveChangesAsync();
                    return true;
                }

                return false;
            }

            return false;
        }


        #endregion

        #region Company Crud

        public async Task<Company> GetClientCompanyByCompanyId(int companyId)
        {
            if (companyId > 0)
            {
                using (var context = new ApplicationDbContext())
                {
                    var companyInDb = await context.Companies.Include(x => x.Client).Include(x => x.CompanyCredits)
                        .FirstOrDefaultAsync(x => x.Id.Equals(companyId));

                    // for getting app-user may or may not need in future depending on page view 
                    if (companyInDb != null)
                    {
                        var userVm = await GetClientUserById(companyInDb.Client.AppUserId);
                        companyInDb.Client.User = userVm.User;
                    }

                    return companyInDb;
                }
            }

            return null;
        }

        public async Task<bool> AddUpdateClientCompanyByCompanyId(CompanyViewModel companyViewModel)
        {
            if (companyViewModel?.Company != null)
            {
                using (var context = new ApplicationDbContext())
                {
                    var companyInDb = await context.Companies.FirstOrDefaultAsync(x => x.Id.Equals(companyViewModel.Company.Id));
                    if (companyInDb == null)
                    {
                        var company = companyViewModel.Company;
                        company.CreatedDate = DateTime.UtcNow;
                        company.UpdatedDate = DateTime.UtcNow;
                        context.Companies.AddOrUpdate(company);


                    }
                    else
                    {
                        companyInDb = companyViewModel.Company;

                        companyInDb.UpdatedDate = DateTime.UtcNow;
                        companyInDb.CreatedDate = DateTime.UtcNow;
                        context.Companies.AddOrUpdate(companyInDb);
                    }

                    await context.SaveChangesAsync();
                    return true;
                }
            }
            return false;
        }


        public async Task<bool> DeleteCompany(int id)
        {
            if (id > 0)
            {
                using (var context = new ApplicationDbContext())
                {
                    var companyInDb = await context.Companies.FirstOrDefaultAsync(x => x.Id.Equals(id));
                    if (companyInDb != null)
                    {
                        if (companyInDb.IsDeleted)
                        {
                            companyInDb.IsDeleted = false;
                        }
                        else
                        {
                            companyInDb.IsDeleted = true;
                        }
                        context.Companies.AddOrUpdate(companyInDb);
                        await context.SaveChangesAsync();
                        return true;
                    }
                    return false;
                }
            }
            return false;
        }


        public async Task<bool> DeleteClient(int id)
        {
            if (id > 0)
            {
                using (var context = new ApplicationDbContext())
                {
                    var clientInDb = await context.Clients.FirstOrDefaultAsync(x => x.Id.Equals(id));
                    if (clientInDb != null)
                    {
                        if (clientInDb.IsDeleted)
                        {
                            clientInDb.IsDeleted = false;
                        }
                        else
                        {
                            clientInDb.IsDeleted = true;
                        }
                        context.Clients.AddOrUpdate(clientInDb);
                        await context.SaveChangesAsync();
                        return true;
                    }
                    return false;
                }
            }
            return false;
        }


        #endregion

        #region LoggedIn User Info Get And Post
        public async Task<UserViewModel> GetLoggedInUserInfo(string id, string userRole)
        {
            Client clientInDb = null;
            if (!string.IsNullOrEmpty(id))
            {
                using (var context = new ApplicationDbContext())
                {
                    var appUser = await context.Users.FirstOrDefaultAsync(x => x.Id.Equals(id));
                    if (!string.IsNullOrEmpty(userRole) && userRole.Equals(RoleName.Client))
                    {
                        clientInDb = await context.Clients.FirstOrDefaultAsync(x => x.AppUserId.Equals(appUser.Id));
                    }
                  
                    var userVm = new UserViewModel
                    {
                        Client = clientInDb,
                        User = appUser,
                        RoleName = userRole
                    };
                    return userVm;
                }
            }
            return null;
        }
        public async Task<bool> UpdateAppUser(UserViewModel userViewModel)
        {
            if (userViewModel?.User != null)
            {
                _context.Users.AddOrUpdate(userViewModel.User);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;

        }


        #endregion
    }

    public interface IClientService
    {
        Task<UserViewModel> GetClientUserById(string id);
        Task<bool> UpdateAppUser(UserViewModel userViewModel);
        Task<UserViewModel> GetLoggedInUserInfo(string id, string userRole);
        Task<bool> UpdateClientAppUser(UserViewModel userViewModel);
        Task<bool> DeleteClient(int id);
        Task<bool> DeleteCompany(int id);
        Task<bool> AddUpdateClientCompanyByCompanyId(CompanyViewModel companyViewModel);
        Task<Company> GetClientCompanyByCompanyId(int companyId);

    }
}
