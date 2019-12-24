using LGS.Data.AppDataProperties;
using LGS.Data.ViewModels.DatabaseViewModels;
using LGS.Models.Analytics;
using LGS.Models.Companies;
using LGS.Models.RoleNames;
using LGS.Models.Users;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;
using LGS.Models.Credits;
using LGS.Models.Items;

namespace LGS.Data.Services.UserServices
{
    public class AdminService : IAdminService
    {
        private readonly ApplicationDbContext _context;

        public AdminService(ApplicationDbContext context)
        {
            _context = context ?? throw new NullReferenceException("Context");
        }

        #region General Admin Service Checks / List Users Miscellaneous Methods 

        public async Task<DashboardViewModel> GetAdminDashboardViewData()
        {
            var users =  await _context.Users.Include(x => x.Roles).ToListAsync();
            var analyticsUnique = await  _context.Database
                .SqlQuery<Analytics_Visits>("select distinct IpAddress FROM Analytics_Visits").ToListAsync();

            var dashboardViewModel = new DashboardViewModel
            {
                Users = users,
                UniqueUsers = analyticsUnique
            };
            return dashboardViewModel;
        }

        public async Task<List<UserViewModel>> GetSubAdminsUserVm(List<UserViewModel> users)
        {
            if (users != null && users.Count > 0)
            {
                foreach (var user in users)
                {
                    var subAdmin = await _context.SubAdmins.FirstOrDefaultAsync(x => x.AppUserId.Equals(user.User.Id));
                    if (subAdmin != null)
                    {
                        user.SubAdmin = subAdmin;
                    }
                }

                return users;
            }

            return null;
        }

        public async Task<List<UserViewModel>> GetClientsUserVm(List<UserViewModel> users)
        {
            if (users != null && users.Count > 0)
            {
                foreach (var user in users)
                {
                    var client = await _context.Clients.FirstOrDefaultAsync(x => x.AppUserId.Equals(user.User.Id));
                    if (client != null)
                    {
                        user.Client = client;
                    }
                }

                return users;
            }

            return null;
        }


        public async Task<bool> CheckUserExistAgainstEmail(string email)
        {
            if (!string.IsNullOrEmpty(email))
            {
                var user = await _context.Users.FirstOrDefaultAsync(x => x.Email.Equals(email));
                if (user == null)
                {
                    return false;
                }

                return true;
            }

            return true;
        }

        public async Task<LgsUserStatus> CheckUserStatus(string email)
        {
            if (!string.IsNullOrEmpty(email))
            {
                var user = await _context.Users.FirstOrDefaultAsync(x => x.Email.Equals(email));
                if (user == null)
                {
                    return LgsUserStatus.UserDoesnotExist;
                }

                var client = await _context.Clients.FirstOrDefaultAsync(x => x.AppUserId.Equals(user.Id));
                var subAdmin = await _context.SubAdmins.FirstOrDefaultAsync(x => x.AppUserId.Equals(user.Id));
                if (client != null)
                {
                    if (client.IsBlocked.Equals(true))
                        return LgsUserStatus.UserBlocked;

                    if (client.IsDeleted.Equals(true))
                        return LgsUserStatus.UserDeleted;
                }

                if (subAdmin != null)
                {
                    if (subAdmin.IsBlocked.Equals(true))
                        return LgsUserStatus.UserBlocked;
                }
            }

            return LgsUserStatus.InvalidRequest;
        }

        #endregion

        #region LoggedIn User Info Get And Post

        public async Task<UserViewModel> GetLoggedInUserInfo(string id, string userRole)
        {
            Client clientInDb = null;
            SubAdmin subAdminInDb = null;
            if (!string.IsNullOrEmpty(id))
            {
                var appUser = await _context.Users.FirstOrDefaultAsync(x => x.Id.Equals(id));
                if (!string.IsNullOrEmpty(userRole) && userRole.Equals(RoleName.Client))
                {
                    clientInDb = await _context.Clients.FirstOrDefaultAsync(x => x.AppUserId.Equals(appUser.Id));
                }

                if (!string.IsNullOrEmpty(userRole) && userRole.Equals(RoleName.SubAdmin))
                {
                    subAdminInDb = await _context.SubAdmins.FirstOrDefaultAsync(x => x.AppUserId.Equals(appUser.Id));
                }

                var userVm = new UserViewModel
                {
                    Client = clientInDb,
                    SubAdmin = subAdminInDb,
                    User = appUser,
                    RoleName = userRole
                };
                return userVm;
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


        #region Company Client SubAdmin Block Methods

        public async Task<bool> BlockSubAdminUser(int id)
        {
            if (id > 0)
            {
                var subAdmin = await _context.SubAdmins.FirstOrDefaultAsync(x => x.Id.Equals(id));
                if (subAdmin != null)
                {
                    if (subAdmin.IsBlocked)
                    {
                        subAdmin.IsBlocked = false;
                    }
                    else
                    {
                        subAdmin.IsBlocked = true;
                    }

                    _context.SubAdmins.AddOrUpdate(subAdmin);
                    await _context.SaveChangesAsync();
                    return subAdmin.IsBlocked;
                }

                return false;
            }

            return false;
        }

        public async Task<bool> BlockClientUser(int id)
        {
            if (id > 0)
            {
                var client = await _context.Clients.FirstOrDefaultAsync(x => x.Id.Equals(id));
                if (client != null)
                {
                    if (client.IsBlocked)
                    {
                        client.IsBlocked = false;
                    }
                    else
                    {
                        client.IsBlocked = true;
                    }

                    _context.Clients.AddOrUpdate(client);
                    await _context.SaveChangesAsync();
                    return client.IsBlocked;
                }

                return false;
            }

            return false;
        }

        public async Task<bool> BlockCompany(int id)
        {
            if (id > 0)
            {
                var company = await _context.Companies.FirstOrDefaultAsync(x => x.Id.Equals(id));
                if (company != null)
                {
                    if (company.IsBlocked)
                    {
                        company.IsBlocked = false;
                    }
                    else
                    {
                        company.IsBlocked = true;
                    }

                    _context.Companies.AddOrUpdate(company);
                    await _context.SaveChangesAsync();
                    return company.IsBlocked;
                }

                return false;
            }

            return false;
        }

        #endregion

        #region Client Crud Methods Admin Service

        public async Task<bool> RegisterClientUser(Client client)
        {
            try
            {
                if (client != null)
                {
                    _context.Clients.Add(client);
                    await _context.SaveChangesAsync();
                    return true;
                }

                return false;
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }

                throw;
            }
        }

        public async Task<UserViewModel> GetClientUserById(int id)
        {
            if (id != 0)
            {
//                        var clientDataResults = (from client in context.Clients 
//                        join companies in context.Companies on client.Id equals companies.ClientId 
//                        join accountCredits in context.AccountCredits on client.User.Id equals accountCredits.UserId
//                        join creditInvoices in context.CreditInvoices on client.User.Id equals creditInvoices.UserId
//                        select new { client, companies, accountCredits, creditInvoices }).ToList();

                var clientInDbWithCompaniesCredits = await _context.Clients.Include(x => x.User)
                    .Include(x => x.Companies).Include(x => x.AccountCredit).Include(x => x.CreditInvoices)
                    .FirstOrDefaultAsync(x => x.Id.Equals(id));

//                    var clientInDb = await context.Clients.Include(x => x.User).FirstOrDefaultAsync(x => x.Id.Equals(id));
//                    var companiesInDb = await context.Companies.Where(c => c.ClientId.Equals(id)).ToListAsync();
//                    var accountCreditInDb = await context.AccountCredits.Where(x => x.UserId.Equals(clientInDb.User.Id)).ToListAsync();
//                    var creditInvoicesInDb = await context.CreditInvoices.Where(x => x.UserId.Equals(clientInDb.User.Id)).ToListAsync();

                var userVm = new UserViewModel
                {
                    Client = clientInDbWithCompaniesCredits,
                    User = clientInDbWithCompaniesCredits?.User
                };
                return userVm;
            }

            return null;
        }


        public async Task<bool> UpdateClientAppUser(UserViewModel userViewModel)
        {
            if (userViewModel?.Client != null)
            {
                var clientAppUser = await GetClientUserById(userViewModel.Client.Id);
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

        public async Task<Company> GetClientCompanyByCompanyId(int companyId)
        {
            if (companyId > 0)
            {
                var companyInDb = await _context.Companies.Include(x => x.Client).Include(x => x.CompanyInvoices)
                    .FirstOrDefaultAsync(x => x.Id.Equals(companyId));

                // for getting app-user may or may not need in future depending on page view 
                if (companyInDb != null)
                {
                    var userVm = await GetClientUserById(companyInDb.Client.Id);
                    companyInDb.Client.User = userVm.User;
                }

                return companyInDb;
            }

            return null;
        }

        public async Task<bool> AddUpdateClientCompanyByCompanyId(CompanyViewModel companyViewModel)
        {
            if (companyViewModel?.Company != null)
            {
                var companyInDb =
                    await _context.Companies.FirstOrDefaultAsync(x => x.Id.Equals(companyViewModel.Company.Id));
                if (companyInDb == null)
                {
                    var company = companyViewModel.Company;
                    company.CreatedDate = DateTime.UtcNow;
                    company.UpdatedDate = DateTime.UtcNow;
                    _context.Companies.AddOrUpdate(company);
                }
                else
                {
                    companyInDb = companyViewModel.Company;

                    companyInDb.UpdatedDate = DateTime.UtcNow;
                    companyInDb.CreatedDate = DateTime.UtcNow;
                    _context.Companies.AddOrUpdate(companyInDb);
                }

                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }


        public async Task<bool> DeleteCompany(int id)
        {
            if (id > 0)
            {
                var companyInDb = await _context.Companies.FirstOrDefaultAsync(x => x.Id.Equals(id));
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

                    _context.Companies.AddOrUpdate(companyInDb);
                    await _context.SaveChangesAsync();
                    return true;
                }

                return false;
            }

            return false;
        }


        public async Task<bool> DeleteClient(int id)
        {
            if (id > 0)
            {
                var clientInDb = await _context.Clients.FirstOrDefaultAsync(x => x.Id.Equals(id));
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

                    _context.Clients.AddOrUpdate(clientInDb);
                    await _context.SaveChangesAsync();
                    return true;
                }

                return false;
            }

            return false;
        }

        #endregion

        #region Sub-Admin Crud Methods Admin Service 

        public async Task<bool> RegisterSubAdminUser(SubAdmin subAdmin)
        {
            try
            {
                if (subAdmin != null)
                {
                    _context.SubAdmins.Add(subAdmin);
                    await _context.SaveChangesAsync();
                    return true;
                }

                return false;
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }

                throw;
            }
        }

        public UserViewModel GetSubAdminUserById(int id)
        {
            if (id != 0)
            {
                var subAdmin = _context.SubAdmins.Include(x => x.User).FirstOrDefault(x => x.Id.Equals(id));
                var userVm = new UserViewModel
                {
                    SubAdmin = subAdmin,
                    User = subAdmin?.User
                };
                return userVm;
            }

            return null;
        }

        public async Task<bool> UpdateSubAdminAppUser(UserViewModel userViewModel)
        {
            if (userViewModel?.SubAdmin != null)
            {
                var subAdminAppUser = GetSubAdminUserById(userViewModel.SubAdmin.Id);
                if (subAdminAppUser != null)
                {
                    subAdminAppUser.SubAdmin.UpdatedDate = DateTime.UtcNow;
                    subAdminAppUser.SubAdmin.ProfilePhoto = userViewModel.SubAdmin.ProfilePhoto;
                    subAdminAppUser.User.FullName = userViewModel.User.FullName;
                    subAdminAppUser.User.Email = userViewModel.User.Email;
                    _context.Users.AddOrUpdate(subAdminAppUser.User);
                    _context.SubAdmins.AddOrUpdate(subAdminAppUser.SubAdmin);
                    await _context.SaveChangesAsync();
                    return true;
                }

                return false;
            }

            return false;
        }

        public async Task<bool> DeleteSubAdminAppUser(int subAdminId)
        {
            if (subAdminId != 0)
            {
                var subAdminAppUser = GetSubAdminUserById(subAdminId);
                if (subAdminAppUser != null)
                {
                    _context.Users.Remove(subAdminAppUser.User);
                    _context.SubAdmins.Remove(subAdminAppUser.SubAdmin);
                    await _context.SaveChangesAsync();
                    return true;
                }

                return false;
            }

            return false;
        }

        #endregion


        #region Get Invoice Details

        public async Task<CreditInvoice> GetInvoiceDetails(int id)
        {
            var creditInvoice =
                await _context.CreditInvoices.Include(x => x.User).FirstOrDefaultAsync(x => x.Id.Equals(id));
            return creditInvoice;
        }

        #endregion


        #region Get Settings

        public async Task<LgsSetting> GetSettings()
        {
            var lgsSetting = await _context.LgsSettings.FirstOrDefaultAsync();
            return lgsSetting;
        }

        public async Task<bool> SaveSettings(LgsSetting lgsSetting)
        {
            lgsSetting.CreatedDate = DateTime.Now;
            lgsSetting.UpdatedDate = DateTime.Now;
            _context.LgsSettings.AddOrUpdate(lgsSetting);
            await _context.SaveChangesAsync();
            return true;
        }

        #endregion
    }

    public interface IAdminService
    {
        Task<bool> RegisterClientUser(Client client);
        Task<bool> RegisterSubAdminUser(SubAdmin subAdmin);
        Task<bool> UpdateSubAdminAppUser(UserViewModel userViewModel);
        Task<bool> UpdateClientAppUser(UserViewModel userViewModel);
        Task<DashboardViewModel> GetAdminDashboardViewData();
        Task<List<UserViewModel>> GetSubAdminsUserVm(List<UserViewModel> users);
        Task<List<UserViewModel>> GetClientsUserVm(List<UserViewModel> users);
        Task<bool> UpdateAppUser(UserViewModel userViewModel);

        Task<bool> CheckUserExistAgainstEmail(string email);
        Task<bool> BlockSubAdminUser(int id);
        Task<bool> BlockClientUser(int id);
        Task<bool> BlockCompany(int id);
        Task<bool> DeleteCompany(int id);
        Task<bool> DeleteClient(int id);
        Task<LgsUserStatus> CheckUserStatus(string email);

        UserViewModel GetSubAdminUserById(int id);
        Task<UserViewModel> GetClientUserById(int id);
        Task<UserViewModel> GetLoggedInUserInfo(string id, string userRole);
        Task<Company> GetClientCompanyByCompanyId(int companyId);
        Task<bool> AddUpdateClientCompanyByCompanyId(CompanyViewModel companyViewModel);
        Task<bool> DeleteSubAdminAppUser(int subAdminId);
        Task<CreditInvoice> GetInvoiceDetails(int id);
        Task<LgsSetting> GetSettings();
        Task<bool> SaveSettings(LgsSetting lgsSetting);
    }
}