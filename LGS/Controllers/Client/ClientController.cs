using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using LGS.AppProperties;
using LGS.Data;
using LGS.Data.Services.ClientServices;
using LGS.Data.ViewModels.DatabaseViewModels;
using LGS.Filters;
using LGS.Helpers.FileUploader;
using LGS.Models.RoleNames;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using DashboardViewModel = LGS.Models.ViewModels.DashboardViewModels.DashboardViewModel;

namespace LGS.Controllers.Client
{
    [Authorize(Roles = RoleName.Client)]
    public class ClientController : Controller
    {
        #region Constructor Inits Service Inits

        private IClientService _clientService;
        private ApplicationUserManager _userManager;

        public ClientController()
        {
        }

        public ClientController(IClientService adminService, ApplicationUserManager userManager
        )
        {
            UserManager = userManager;
            Service = adminService ?? throw new ArgumentNullException("adminService");
        }

        public IClientService Service
        {
            get { return _clientService = new ClientService(new ApplicationDbContext()); }
            private set { _clientService = value; }
        }


        public ApplicationUserManager UserManager
        {
            get { return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
            private set { _userManager = value; }
        }

        #endregion

        public async Task<ActionResult> Index()
        {
            if (TempData[AppConstants.AlertDialog] == null)
                TempData[AppConstants.AlertDialog] = 0;
            ViewBag.AlertDialog = (int) TempData[AppConstants.AlertDialog];
            var loggedInUserId = User.Identity.GetUserId();

            if (!string.IsNullOrEmpty(loggedInUserId))
            {
                var clientUserVm = await Service.GetClientUserById(loggedInUserId);
                if (clientUserVm != null)
                {
                    var clientDashboardVm = new DashboardViewModel
                    {
                        UserVm = clientUserVm
                    };
                    return View(clientDashboardVm);
                }
            }

            return View();
        }




        #region Company Details Page Get And Post Methods

        public async Task<ActionResult> CompanyDetail(int id, int clientId)
        {
            if (TempData[AppConstants.AlertDialog] == null)
                TempData[AppConstants.AlertDialog] = 0;
            ViewBag.AlertDialog = (int) TempData[AppConstants.AlertDialog];

            if (id > 0)
            {
                var companyInDb = await Service.GetClientCompanyByCompanyId(id);
                if (companyInDb != null)
                {
                    var userViewModel = new UserViewModel
                    {
                        Client = companyInDb?.Client,
                        User = companyInDb?.Client.User
                    };

                    var companyDetailViewModel = new CompanyViewModel
                    {
                        Company = companyInDb,
                        UserViewModel = userViewModel
                    };
                    return View(companyDetailViewModel);
                }

                return RedirectToAction("Index", "client");
            }

            return RedirectToAction("Index", "client");
        }

        [HttpPost, ValidateInput(false)]
        public async Task<ActionResult> CompanyDetailUpdate(CompanyViewModel companyViewModel)
        {
            if (companyViewModel != null)
            {
                if (companyViewModel.Company != null)
                {
                    if (companyViewModel.CompanyLogoPic != null)
                    {
                        var fileUploadHelper = new FileUploadHelper();

                        var uploads = Path.Combine(Server.MapPath("~/LgsImageRepo/CompanyImages"));
                        var profileImagePath = fileUploadHelper.CompanySaveFile(companyViewModel.CompanyLogoPic,
                            uploads,
                            companyViewModel.Company.CompanyEmail);
                        var profileImagePathOnServer =
                            profileImagePath.Replace(Server.MapPath("~/"), "/").Replace("\\", "/");
                        companyViewModel.Company.LogoPath = profileImagePathOnServer;
                    }


                    var companyIsUpdated = await Service.AddUpdateClientCompanyByCompanyId(companyViewModel);

                    TempData[AppConstants.AlertDialog] = LgsAlertEnums.SuccessfulUpdate;
                    return RedirectToAction("companydetail", "client",
                        new {id = companyViewModel.Company.Id, clientId = companyViewModel.Company.ClientId});
                }


                TempData[AppConstants.AlertDialog] = LgsAlertEnums.InvalidModel;
                return RedirectToAction("companydetail", "client",
                    new {id = companyViewModel.Company.Id, clientId = companyViewModel.Company.ClientId});

                //                return RedirectToAction("ClientDetails", "Admin", new {id = companyViewModel.Company.ClientId});
            }

            TempData[AppConstants.AlertDialog] = LgsAlertEnums.InvalidModel;
            return RedirectToAction("companydetail", "client",
                new {id = companyViewModel.Company.Id, clientId = companyViewModel.Company.ClientId});
            //            return RedirectToAction("ClientDetails", "Admin", new {id = companyViewModel.Company.ClientId});
        }

        #endregion

        #region Profile Page Region 

        [HttpGet]
        public async Task<ActionResult> ProfilePage()
        {
            if (TempData[AppConstants.AlertDialog] == null)
                TempData[AppConstants.AlertDialog] = 0;
            ViewBag.AlertDialog = (int) TempData[AppConstants.AlertDialog];

            var loggedInUserId = User.Identity.GetUserId();
            var userRole = await UserManager.GetRolesAsync(loggedInUserId);
            var userVm = await Service.GetLoggedInUserInfo(loggedInUserId, userRole.First());
            if (userVm != null)
            {
                var userDashboardVm = new DashboardViewModel
                {
                    UserVm = userVm,
                };
                return View(userDashboardVm);
            }

            return View();
        }


        [HttpPost]
        public async Task<ActionResult> ProfilePage(DashboardViewModel dashboardViewModel)
        {
            var loggedInUserId = User.Identity.GetUserId();
            dashboardViewModel.UserVm.Client.AppUserId = loggedInUserId;
            if (dashboardViewModel?.UserVm != null && dashboardViewModel.IsEnable)
            {
                if (dashboardViewModel.UserVm.Client != null &&
                    dashboardViewModel.UserVm.RoleName.Equals(RoleName.Client))
                {
                    if (dashboardViewModel.ProfilePic != null)
                    {
                        var fileUploadHelper = new FileUploadHelper();

                        var uploads = Path.Combine(Server.MapPath("~/LgsImageRepo/ProfileImages"));
                        var profileImagePath = fileUploadHelper.SaveFile(dashboardViewModel.ProfilePic, uploads,
                            dashboardViewModel.UserVm.User.Email);
                        var profileImagePathOnServer =
                            profileImagePath.Replace(Server.MapPath("~/"), "/")
                                .Replace("\\",
                                    "/"); //Relative Path can be stored in database or do logically what is needed.
                        dashboardViewModel.UserVm.Client.ProfilePhoto = profileImagePathOnServer;
                        dashboardViewModel.UserVm.User.UserProfilePic = profileImagePathOnServer;
                    }

                    var isUpdated = await Service.UpdateClientAppUser(dashboardViewModel.UserVm);
                    if (isUpdated)
                    {
                        TempData[AppConstants.AlertDialog] = LgsAlertEnums.SuccessfulUpdate;
                        return RedirectToAction("profilepage", "client");
                    }

                    TempData[AppConstants.AlertDialog] = LgsAlertEnums.InvalidModel;
                    return RedirectToAction("profilepage", "client");
                }
            }

            TempData[AppConstants.AlertDialog] = LgsAlertEnums.InvalidModel;
            return RedirectToAction("profilepage");
        }




        // Reset Password
        [HttpPost]
        [ValidateInput(true)]
        public async Task<ActionResult> ResetPassword(DashboardViewModel dashboardViewModel)
        {
            var user = await UserManager.FindByNameAsync(dashboardViewModel.UserVm.User.Email);
            if (user == null)
            {
                TempData[AppConstants.AlertDialog] = LgsAlertEnums.InvalidModel;
                // Don't reveal that the user does not exist
                return RedirectToAction("profilepage", "client");
            }
            string resetToken = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
            var result = await UserManager.ResetPasswordAsync(user.Id, resetToken, dashboardViewModel.Password);
            if (result.Succeeded)
            {
                TempData[AppConstants.AlertDialog] = LgsAlertEnums.SuccessfulUpdate;
                return RedirectToAction("profilepage", "client");
            }

            TempData[AppConstants.AlertDialog] = LgsAlertEnums.InvalidModel;
            return RedirectToAction("profilepage");
        }
        #endregion

        #region Add Company
        public async Task<ActionResult> AddCompany()
        {
            if (TempData[AppConstants.AlertDialog] == null)
                TempData[AppConstants.AlertDialog] = 0;
            ViewBag.AlertDialog = (int)TempData[AppConstants.AlertDialog];
            var loggedInUserId = User.Identity.GetUserId();
            var userRole = await UserManager.GetRolesAsync(loggedInUserId);
            var user = await Service.GetLoggedInUserInfo(loggedInUserId, userRole.First());
            var companyViewModel = new CompanyViewModel
            {
                UserViewModel = user
            };
            return View(companyViewModel);
        }


        #endregion
    }
}