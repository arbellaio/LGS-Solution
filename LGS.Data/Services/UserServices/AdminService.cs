using System;
using System.Collections.Generic;
using System.Data.Entity;
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
            var analyticsUnique = await _context.Database.SqlQuery<Analytics_Visits>("SELECT DISTINCT IpAddress FROM Analytics_Visits").ToListAsync();

            var dashboardViewModel = new DashboardViewModel
            {
                Users = users,
                UniqueUsers = analyticsUnique
            };
            return dashboardViewModel;
        }

        public async Task<List<UserViewModel>> GetSubAdminsUserVm(List<UserViewModel> users)
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

        public async Task<List<UserViewModel>> GetClientsUserVm(List<UserViewModel> users)
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
    }

    public interface IAdminService
    {
        Task<bool> RegisterClientUser(Client client);
        Task<bool> RegisterSubAdminUser(SubAdmin client);
        Task<DashboardViewModel> GetAdminDashboardViewData();
        Task<List<UserViewModel>> GetSubAdminsUserVm(List<UserViewModel> users);
        Task<List<UserViewModel>> GetClientsUserVm(List<UserViewModel> users);

//        Task<bool> GetClientUserByUserEmail();
//        Task<bool> GetClientUserByAppUserId();
//        Task<bool> GetClientUserByClientUserId();
//        Task<bool> GetCustomerUserByCustomerUserId();
//        Task<bool> GetCustomerUserByAppUserId();
    }
}