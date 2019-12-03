using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LGS.Data.ViewModels.DatabaseViewModels;
using LGS.Models.Communication;
using LGS.Models.Companies;
using LGS.Models.Credits;
using LGS.Models.PaypalItem;
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
                var clientInDbWithCompaniesCredits = await _context.Clients.Include(x => x.User)
                    .Include(x => x.Companies).Include(x => x.AccountCredit).Include(x => x.CreditInvoices)
                    .FirstOrDefaultAsync(x => x.AppUserId.Equals(id));
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
                _context.Configuration.ProxyCreationEnabled = false;

                var companyInDb = await _context.Companies.Include(x => x.Client).Include(x => x.CompanyInvoices).Include(x => x.CustomerReviews).Include(x => x.CustomerMessages)
                    .FirstOrDefaultAsync(x => x.Id.Equals(companyId));

                // for getting app-user may or may not need in future depending on page view 
                if (companyInDb != null)
                {
                    var userVm = await GetClientUserById(companyInDb.Client.AppUserId);
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

        #region LoggedIn User Info Get And Post

        public async Task<UserViewModel> GetLoggedInUserInfo(string id, string userRole)
        {
            Client clientInDb = null;
            if (!string.IsNullOrEmpty(id))
            {
                var appUser = await _context.Users.FirstOrDefaultAsync(x => x.Id.Equals(id));
                if (!string.IsNullOrEmpty(userRole) && userRole.Equals(RoleName.Client))
                {
                    clientInDb = await _context.Clients.FirstOrDefaultAsync(x => x.AppUserId.Equals(appUser.Id));
                }

                var userVm = new UserViewModel
                {
                    Client = clientInDb,
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


        #region Purchases Invoices

        public async Task<bool> AddInvoice(CreditInvoice creditInvoice, AccountCredit accountCredit)
        {
            if (creditInvoice != null && accountCredit != null)
            {
                _context.AccountCredits.AddOrUpdate(accountCredit);
                _context.CreditInvoices.AddOrUpdate(creditInvoice);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        #endregion

        #region Get Invoice Details

        public async Task<CreditInvoice> GetInvoiceDetails(int id)
        {
            _context.Configuration.ProxyCreationEnabled = false;
            var creditInvoice =
                await _context.CreditInvoices.Include(x => x.User).FirstOrDefaultAsync(x => x.Id.Equals(id));
            return creditInvoice;
        }

        #endregion

        #region AddCompanyPurchase

        public async Task<bool> AddCompanyPurchase(AccountCredit accountCredit, CompanyInvoice companyInvoice,
            CompanyInventory companyInventory)
        {
            if (accountCredit != null && companyInventory != null && companyInvoice != null)
            {
                _context.AccountCredits.AddOrUpdate(accountCredit);
                _context.CompanyInventories.AddOrUpdate(companyInventory);
                _context.CompanyInvoices.AddOrUpdate(companyInvoice);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        #endregion

        #region Account Credits Methods

        public async Task<AccountCredit> GetAccountCredit(int clientId)
        {
            _context.Configuration.ProxyCreationEnabled = false;
            var accountCredit = await _context.AccountCredits.FirstOrDefaultAsync(x => x.ClientId.Equals(clientId));
            return accountCredit;
        }

        public async Task<CompanyInventory> GetCompanyInventory(int companyId)
        {
            _context.Configuration.ProxyCreationEnabled = false;
            var companyInventory =
                await _context.CompanyInventories.FirstOrDefaultAsync(x => x.CompanyId.Equals(companyId));
            return companyInventory;
        }

        #endregion

        #region Set Company Settings

        public async Task<bool> SaveCompanySettings(Company company)
        {
            if (company != null && company.Id > 0)
            {
                _context.Configuration.ProxyCreationEnabled = false;
                var registeredCompany = await _context.Companies.FirstOrDefaultAsync(x => x.Id.Equals(company.Id));
                if (registeredCompany != null)
                {
                    registeredCompany.NotificationMode = company.NotificationMode;
                    registeredCompany.DeliveryInterval = company.DeliveryInterval;
                    registeredCompany.LeadLimit = company.LeadLimit;
                    _context.Companies.AddOrUpdate(registeredCompany);
                    await _context.SaveChangesAsync();
                    return true;
                }

                return false;
            }

            return false;
        }

        #endregion


        public async Task<List<CustomerReview>> GetCustomerReviews(string email)
        {
            if (!string.IsNullOrEmpty(email))
            {
                _context.Configuration.ProxyCreationEnabled = false;

                return await _context.CustomerReviews.Where(x => x.CustomerEmail.Equals(email)).ToListAsync();
            }

            return null;
        }

        public async Task<CustomerMessage> GetCustomerMessage( int messageId)
        {
            if (messageId > 0)
            {
                _context.Configuration.ProxyCreationEnabled = false;

                var message = await _context.CustomerMessages.FirstOrDefaultAsync(x => x.Id.Equals(messageId));
               return message;
            }

            return null;
        }

    }

    public interface IClientService
    {
        Task<bool> UpdateAppUser(UserViewModel userViewModel);
        Task<bool> UpdateClientAppUser(UserViewModel userViewModel);

        Task<bool> AddUpdateClientCompanyByCompanyId(CompanyViewModel companyViewModel);
        Task<bool> AddInvoice(CreditInvoice creditInvoice, AccountCredit accountCredit);

        Task<bool> AddCompanyPurchase(AccountCredit accountCredit, CompanyInvoice companyInvoice,
            CompanyInventory companyInventory);

        Task<UserViewModel> GetClientUserById(string id);
        Task<UserViewModel> GetLoggedInUserInfo(string id, string userRole);
        Task<Company> GetClientCompanyByCompanyId(int companyId);
        Task<CreditInvoice> GetInvoiceDetails(int id);
        Task<AccountCredit> GetAccountCredit(int clientId);
        Task<CompanyInventory> GetCompanyInventory(int companyId);

        Task<bool> SaveCompanySettings(Company company);
        Task<List<CustomerReview>> GetCustomerReviews(string email);
        Task<CustomerMessage> GetCustomerMessage(int messageId);
        Task<bool> DeleteClient(int id);
        Task<bool> DeleteCompany(int id);
    }

   
}