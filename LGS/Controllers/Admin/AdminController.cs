using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using LGS.Data;
using LGS.Data.Services.UserServices;
using LGS.Data.ViewModels.AdminViewModels;
using LGS.Helpers.UsersHelper;
using LGS.Models;
using LGS.Models.AdminViewModels.DashboardViewModels;
using LGS.Models.RoleNames;
using LGS.Models.Users;
using Microsoft.AspNet.Identity.Owin;
using DashboardViewModel = LGS.Models.AdminViewModels.DashboardViewModels.DashboardViewModel;

namespace LGS.Controllers.Admin
{
    [Authorize(Roles = RoleName.Admin)]
    public class AdminController : Controller
    {
        private IAdminService _adminService;
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AdminController()
        {
        }

        public AdminController(IAdminService adminService, ApplicationUserManager userManager,
            ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            Service = adminService ?? throw new ArgumentNullException("adminService");
        }

        public IAdminService Service
        {
            get { return _adminService = new AdminService(new ApplicationDbContext()); }
            private set { _adminService = value; }
        }

        public ApplicationSignInManager SignInManager
        {
            get { return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>(); }
            private set { _signInManager = value; }
        }

        public ApplicationUserManager UserManager
        {
            get { return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
            private set { _userManager = value; }
        }

        public async Task<ActionResult> Index()
        {
            //Get Dashboard Data
            var dashboardViewModel = await Service.GetAdminDashboardViewData();
            if (dashboardViewModel?.Users != null && dashboardViewModel.Users.Count > 0)
            {
                //Get users with user role
                var userViewModelList = await GetUsersWithRoles(dashboardViewModel.Users);
                if (userViewModelList != null && userViewModelList.Count > 0)
                {
                    var subAdmins = UserHelper.GetUsersWithSpecificRole(userViewModelList, RoleName.SubAdmin);
                    var clients = UserHelper.GetUsersWithSpecificRole(userViewModelList, RoleName.Client);

                    var dashboardVm = new DashboardViewModel
                    {
                        RegisteredSubAdmins = subAdmins.Count(),
                        RegisteredClients = clients.Count(),
                        UniqueUsers = dashboardViewModel.UniqueUsers.Count(),
                        UserViewModels = userViewModelList
                    };
                    return View(dashboardVm);
                }
            }

            return View();
        }


        public async Task<ActionResult> SubAdminIndex()
        {
            //Get Dashboard Data
            var dashboardViewModel = await Service.GetAdminDashboardViewData();

            //Get users with user role
            var userViewModelList = await GetUsersWithRoles(dashboardViewModel.Users);
            var subAdminsUserVm = UserHelper.GetUsersWithSpecificRole(userViewModelList, RoleName.SubAdmin);
            subAdminsUserVm = await Service.GetSubAdminsUserVm(subAdminsUserVm);
            if (subAdminsUserVm != null)
            {
                var dashboardVm = new DashboardViewModel
                {
                    UserViewModels = subAdminsUserVm
                };
                return View(dashboardVm);
            }

            return View();
        }


        

        public async Task<ActionResult> ClientIndex()
        {
            //Get Dashboard Data
            var dashboardViewModel = await Service.GetAdminDashboardViewData();

            //Get users with user role
            var userViewModelList = await GetUsersWithRoles(dashboardViewModel.Users);
            var clientsUserVm = UserHelper.GetUsersWithSpecificRole(userViewModelList, RoleName.Client);
            clientsUserVm = await Service.GetClientsUserVm(clientsUserVm);
            if (clientsUserVm != null)
            {
                var dashboardVm = new DashboardViewModel
                {
                    UserViewModels = clientsUserVm
                };
                return View(dashboardVm);
            }

            return View();
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ClientRegister(DashboardViewModel dashboardViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = dashboardViewModel.RegisterVm.Email, Email = dashboardViewModel.RegisterVm.Email, FullName = dashboardViewModel.RegisterVm.FullName };
                var result = await UserManager.CreateAsync(user, dashboardViewModel.RegisterVm.Password);
                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    var client = new Client
                    {
                        AppUserId = user.Id,
                        CreatedDate = DateTime.UtcNow,
                        UpdatedDate = DateTime.UtcNow,
                    };
                    var isClientAdded = await Service.RegisterClientUser(client);
                    Console.WriteLine("Client is Register :" + isClientAdded);
                    // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");
                    await this.UserManager.AddToRoleAsync(user.Id, RoleName.Client);
                    return RedirectToAction("ClientIndex", "Admin");
                }

            }

            // If we got this far, something failed, redisplay form
            return RedirectToAction("ClientIndex","Admin");
        }




        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SubAdminRegister(DashboardViewModel dashboardViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = dashboardViewModel.RegisterVm.Email, Email = dashboardViewModel.RegisterVm.Email, FullName = dashboardViewModel.RegisterVm.FullName };
                var result = await UserManager.CreateAsync(user, dashboardViewModel.RegisterVm.Password);
                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    var subAdmin = new SubAdmin
                    {
                        AppUserId = user.Id,
                        CreatedDate = DateTime.UtcNow,
                        UpdatedDate = DateTime.UtcNow,
                    };
                    var isSubAdminAdded = await Service.RegisterSubAdminUser(subAdmin);
                    Console.WriteLine("SubAdmin is Register :" + isSubAdminAdded);
                    // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");
                    await this.UserManager.AddToRoleAsync(user.Id, RoleName.SubAdmin);
                    return RedirectToAction("SubAdminIndex", "Admin");
                }

            }

            // If we got this far, something failed, redisplay form
            return RedirectToAction("SubAdminIndex", "Admin");
        }



        public async Task<ActionResult> SubAdminDetail()
        {

        }


        public ActionResult LiveChat()
        {

            return View("LiveChatIndex");
        }


        public async Task<List<UserViewModel>> GetUsersWithRoles(List<ApplicationUser> users)
        {
            var userViewModelList = new List<UserViewModel>();
            foreach (var user in users)
            {
                var roleName = await UserManager.GetRolesAsync(user.Id);
                var userViewModel = new UserViewModel
                {
                    User = user,
                    RoleName = roleName.First()
                };
                userViewModelList.Add(userViewModel);
            }

            return userViewModelList;
        }
    }
}