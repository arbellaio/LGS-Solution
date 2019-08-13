using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LGS.Data.ViewModels;
using LGS.Data.ViewModels.AdminViewModels;
using LGS.Models;
using LGS.Models.Analytics;
using LGS.Models.RoleNames;
using LGS.Models.Users;

namespace LGS.Data.Services.UserServices
{
    public class AdminService : IAdminService
    {
        private readonly ApplicationDbContext _context;

        public AdminService(ApplicationDbContext context)
        {
            _context = context ?? throw new NullReferenceException("Context");
        }

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

        public async Task<DashboardViewModel> GetAdminDashboardViewData()
        {
            var users = await _context.Users.Include(x => x.Roles).ToListAsync();
            var analyticsUnique = await _context.Database.SqlQuery<Analytics_Visits>("select distinct IpAddress FROM Analytics_Visits").ToListAsync();

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

        public async Task<UserViewModel> GetSubAdminUserById(int id)
        {
            if (id != 0)
            {
                var subAdmin = await _context.SubAdmins.Include(x => x.User).FirstOrDefaultAsync(x => x.Id.Equals(id));
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
                var subAdminAppUser = await GetSubAdminUserById(userViewModel.SubAdmin.Id);
                if (subAdminAppUser != null)
                {
                    subAdminAppUser.SubAdmin.UpdatedDate = DateTime.UtcNow;
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

    }

    public interface IAdminService
    {
        Task<bool> RegisterClientUser(Client client);
        Task<bool> RegisterSubAdminUser(SubAdmin subAdmin);
        Task<bool> UpdateSubAdminAppUser(UserViewModel userViewModel);
        Task<DashboardViewModel> GetAdminDashboardViewData();
        Task<List<UserViewModel>> GetSubAdminsUserVm(List<UserViewModel> users);
        Task<List<UserViewModel>> GetClientsUserVm(List<UserViewModel> users);

        Task<bool> CheckUserExistAgainstEmail(string email);
        Task<UserViewModel> GetSubAdminUserById(int id);


    }
}