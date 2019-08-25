using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using LGS.AppProperties;
using LGS.Data;
using LGS.Data.Services.UserServices;
using LGS.Data.ViewModels.AdminViewModels;
using LGS.Extensions;
using LGS.Helpers.FileUploader;
using LGS.Helpers.UsersHelper;
using LGS.Models;
using LGS.Models.AdminViewModels.DashboardViewModels;
using LGS.Models.RoleNames;
using LGS.Models.Users;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using DashboardViewModel = LGS.Models.AdminViewModels.DashboardViewModels.DashboardViewModel;

namespace LGS.Controllers.Admin
{
    [Authorize(Roles = RoleName.Admin + "," + RoleName.SubAdmin)]
    public class AdminController : Controller
    {
        #region Constructor Inits Service Inits

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

        #endregion


        #region Admin Index Page

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

        #endregion


        #region Client Index / List Page

        public async Task<ActionResult> ClientIndex()
        {
            if (TempData[AppConstants.AlertDialog] == null)
                TempData[AppConstants.AlertDialog] = 0;
            ViewBag.AlertDialog = (int) TempData[AppConstants.AlertDialog];
            //Get Dashboard Data
            var dashboardViewModel = await Service.GetAdminDashboardViewData();

            //Get users with user role
            var userViewModelList = await GetUsersWithRoles(dashboardViewModel.Users);
            var clientsUserVm = UserHelper.GetUsersWithSpecificRole(userViewModelList, RoleName.Client);
            clientsUserVm = await Service.GetClientsUserVm(clientsUserVm);
            clientsUserVm = clientsUserVm.FindAll(x => x.Client.IsDeleted.Equals(false));

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
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ClientRegister(DashboardViewModel dashboardViewModel)
        {
            if (ModelState.IsValid)
            {
                if (!await Service.CheckUserExistAgainstEmail(dashboardViewModel.RegisterVm.Email))
                {
                    var user = new ApplicationUser
                    {
                        UserName = dashboardViewModel.RegisterVm.Email,
                        Email = dashboardViewModel.RegisterVm.Email,
                        FullName = dashboardViewModel.RegisterVm.FullName
                    };
                    var result = await UserManager.CreateAsync(user, dashboardViewModel.RegisterVm.Password);
                    if (result.Succeeded)
                    {
                        //                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
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
                        TempData[AppConstants.AlertDialog] = LgsAlertEnums.SuccessfulRegistration;
                        return RedirectToAction("clientindex", "admin");
                    }
                }

                TempData[AppConstants.AlertDialog] = LgsAlertEnums.UserExist;
                return RedirectToAction("clientindex", "admin");
            }

            TempData[AppConstants.AlertDialog] = LgsAlertEnums.InvalidModel;
            // If we got this far, something failed, redisplay form
            return RedirectToAction("clientindex", "admin");
        }

        #endregion


        #region Sub-Admin Index / List Page

        [Authorize(Roles = RoleName.Admin)]
        public async Task<ActionResult> SubAdminIndex()
        {
            if (TempData[AppConstants.AlertDialog] == null)
                TempData[AppConstants.AlertDialog] = 0;
            ViewBag.AlertDialog = (int) TempData[AppConstants.AlertDialog];

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

            return View("Index");
        }

        [Authorize(Roles = RoleName.Admin)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SubAdminRegister(DashboardViewModel dashboardViewModel)
        {
            if (ModelState.IsValid)
            {
                if (!await Service.CheckUserExistAgainstEmail(dashboardViewModel.RegisterVm.Email))
                {
                    var user = new ApplicationUser
                    {
                        UserName = dashboardViewModel.RegisterVm.Email,
                        Email = dashboardViewModel.RegisterVm.Email,
                        FullName = dashboardViewModel.RegisterVm.FullName
                    };
                    var result = await UserManager.CreateAsync(user, dashboardViewModel.RegisterVm.Password);
                    if (result.Succeeded)
                    {
                        //                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
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

                        TempData[AppConstants.AlertDialog] = LgsAlertEnums.SuccessfulRegistration;
                        return RedirectToAction("subadminindex", "admin");
                    }
                }

                TempData[AppConstants.AlertDialog] = LgsAlertEnums.UserExist;
                return RedirectToAction("subadminindex", "admin");
            }

            TempData[AppConstants.AlertDialog] = LgsAlertEnums.InvalidModel;
            return RedirectToAction("subadminindex", "admin");
        }

        #endregion


        #region Sub-Admin Details Page Get And Post Methods

        [Authorize(Roles = RoleName.Admin)]
        public async Task<ActionResult> SubAdminDetails(int id)
        {
            if (id == 0) return Json("");
            var userVm = await Service.GetSubAdminUserById(id);
            return Json(userVm, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = RoleName.Admin)]
        [HttpPost]
        public async Task<ActionResult> SubAdminDetailsUpdate(DashboardViewModel dashboardViewModel)
        {
            if ((dashboardViewModel?.RegisterVm != null && dashboardViewModel.IsEnable &&
                 dashboardViewModel.SubAdminUserId > 0) ||
                (dashboardViewModel?.ProfilePic != null && dashboardViewModel.IsEnable &&
                 dashboardViewModel.SubAdminUserId > 0))
            {
                var userVmInDb = await Service.GetSubAdminUserById(dashboardViewModel.SubAdminUserId);
                if (userVmInDb?.User != null && userVmInDb.SubAdmin != null)
                {
                    if (dashboardViewModel.RegisterVm != null &&
                        (!string.IsNullOrEmpty(dashboardViewModel.RegisterVm.FullName) &&
                         !string.IsNullOrEmpty(dashboardViewModel.RegisterVm.Email)))
                    {
                        userVmInDb.User.FullName = dashboardViewModel.RegisterVm.FullName;
                        userVmInDb.User.Email = dashboardViewModel.RegisterVm.Email;
                    }

                    if (dashboardViewModel.ProfilePic != null)
                    {
                        var fileUploadHelper = new FileUploadHelper();

                        var uploads = Path.Combine(Server.MapPath("~/LgsImageRepo/ProfileImages"));
                        var profileImagePath = fileUploadHelper.SaveFile(dashboardViewModel.ProfilePic, uploads,
                            userVmInDb.User.Email);
                        var profileImagePathOnServer =
                            profileImagePath.Replace(Server.MapPath("~/"), "/")
                                .Replace("\\",
                                    "/"); //Relative Path can be stored in database or do logically what is needed.
                        userVmInDb.SubAdmin.ProfilePhoto = profileImagePathOnServer;
                        userVmInDb.User.UserProfilePic = profileImagePathOnServer;
                    }


                    var isUpdated = await Service.UpdateSubAdminAppUser(userVmInDb);
                    if (isUpdated)
                    {
                        TempData[AppConstants.AlertDialog] = LgsAlertEnums.SuccessfulUpdate;
                        return RedirectToAction("subadminindex", "admin");
                    }

                    TempData[AppConstants.AlertDialog] = LgsAlertEnums.InvalidModel;
                    return RedirectToAction("subadminindex", "admin");
                }
            }

            TempData[AppConstants.AlertDialog] = LgsAlertEnums.InvalidModel;
            return RedirectToAction("subadminindex", "admin");
        }

        #endregion


        #region Client Details Page Get And Post Methods

        [HttpGet]
        public async Task<ActionResult> ClientDetails(int id)
        {
            if (TempData[AppConstants.AlertDialog] == null)
                TempData[AppConstants.AlertDialog] = 0;
            ViewBag.AlertDialog = (int) TempData[AppConstants.AlertDialog];

            if (id > 0)
            {
                var clientUserVm = await Service.GetClientUserById(id);
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


        [HttpPost]
        public async Task<ActionResult> ClientProfileUpdate(DashboardViewModel dashboardViewModel)
        {
            if ((dashboardViewModel?.RegisterVm != null && dashboardViewModel.IsEnable &&
                 dashboardViewModel.ClientUserId > 0) || (dashboardViewModel?.ProfilePic != null &&
                                                          dashboardViewModel.IsEnable &&
                                                          dashboardViewModel.ClientUserId > 0))
            {
                var userVmInDb = await Service.GetClientUserById(dashboardViewModel.ClientUserId);
                if (userVmInDb?.User != null && userVmInDb.Client != null)
                {
                    if (dashboardViewModel.RegisterVm != null &&
                        (!string.IsNullOrEmpty(dashboardViewModel.RegisterVm.FullName) &&
                         !string.IsNullOrEmpty(dashboardViewModel.RegisterVm.Email)))
                    {
                        userVmInDb.User.FullName = dashboardViewModel.RegisterVm.FullName;
                        userVmInDb.User.Email = dashboardViewModel.RegisterVm.Email;
                    }

                    if (dashboardViewModel.ProfilePic != null)
                    {
                        var fileUploadHelper = new FileUploadHelper();

                        var uploads = Path.Combine(Server.MapPath("~/LgsImageRepo/ProfileImages"));
                        var profileImagePath = fileUploadHelper.SaveFile(dashboardViewModel.ProfilePic, uploads,
                            userVmInDb.User.Email);
                        var profileImagePathOnServer =
                            profileImagePath.Replace(Server.MapPath("~/"), "/").Replace("\\", "/");
                        userVmInDb.Client.ProfilePhoto = profileImagePathOnServer;
                        userVmInDb.User.UserProfilePic = profileImagePathOnServer;
                    }


                    var isUpdated = await Service.UpdateClientAppUser(userVmInDb);
                    if (isUpdated)
                    {
                        TempData[AppConstants.AlertDialog] = LgsAlertEnums.SuccessfulUpdate;
                        return RedirectToAction("clientdetails", "admin", new {id = dashboardViewModel?.ClientUserId});
                    }

                    TempData[AppConstants.AlertDialog] = LgsAlertEnums.InvalidModel;
                    return RedirectToAction("clientdetails", "admin", new {id = dashboardViewModel?.ClientUserId});
                }
            }

            TempData[AppConstants.AlertDialog] = LgsAlertEnums.InvalidModel;
            return RedirectToAction("clientdetails", "admin", new {id = dashboardViewModel?.ClientUserId});
        }

        #endregion


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

                return RedirectToAction("clientdetails", "admin", new {id = clientId});
            }

            return RedirectToAction("clientdetails", "admin", new {id = clientId});
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
                    return RedirectToAction("companydetail", "admin",
                        new {id = companyViewModel.Company.Id, clientId = companyViewModel.Company.ClientId});
                }


                TempData[AppConstants.AlertDialog] = LgsAlertEnums.InvalidModel;
                return RedirectToAction("companydetail", "admin",
                    new {id = companyViewModel.Company.Id, clientId = companyViewModel.Company.ClientId});

//                return RedirectToAction("ClientDetails", "Admin", new {id = companyViewModel.Company.ClientId});
            }

            TempData[AppConstants.AlertDialog] = LgsAlertEnums.InvalidModel;
            return RedirectToAction("companydetail", "admin",
                new {id = companyViewModel.Company.Id, clientId = companyViewModel.Company.ClientId});
//            return RedirectToAction("ClientDetails", "Admin", new {id = companyViewModel.Company.ClientId});
        }

        #endregion


        #region Block / Delete Client && Company

        [HttpGet]
        public async Task<ActionResult> BlockClientUser(int id)
        {
            if (id > 0)
            {
                var isBlocked = await Service.BlockClientUser(id);
                if (isBlocked)
                {
                    TempData[AppConstants.AlertDialog] = LgsAlertEnums.SuccessfulBlock;
                }
                else
                {
                    TempData[AppConstants.AlertDialog] = LgsAlertEnums.SuccessfulUnBlock;
                }

                return RedirectToAction("clientdetails", "admin", new {id});
            }

            TempData[AppConstants.AlertDialog] = LgsAlertEnums.InvalidModel;
            return RedirectToAction("clientindex");
        }

        [HttpGet]
        public async Task<ActionResult> BlockCompany(int id, int clientId)
        {
            if (id > 0)
            {
                var isBlocked = await Service.BlockCompany(id);
                if (isBlocked)
                {
                    TempData[AppConstants.AlertDialog] = LgsAlertEnums.SuccessfulBlock;
                }
                else
                {
                    TempData[AppConstants.AlertDialog] = LgsAlertEnums.SuccessfulUnBlock;
                }

                return RedirectToAction("companydetail", "admin", new {id, clientId});
            }

            TempData[AppConstants.AlertDialog] = LgsAlertEnums.InvalidModel;
            return RedirectToAction("clientindex");
        }

//        [HttpGet]
//        public async Task<ActionResult> DeleteCompany(int id)
//        {
//            if (id > 0)
//            {
//                var isBlocked = await Service.DeleteCompany(id);
//                TempData[AppConstants.AlertDialog] = LgsAlertEnums.SuccessfulDelete;
//                return RedirectToAction("clientindex");
//            }
//
//            TempData[AppConstants.AlertDialog] = LgsAlertEnums.InvalidModel;
//            return RedirectToAction("clientindex");
//        }

        [HttpGet]
        public async Task<ActionResult> DeleteClient(int id)
        {
            if (id > 0)
            {
                var isBlocked = await Service.DeleteClient(id);
                TempData[AppConstants.AlertDialog] = LgsAlertEnums.SuccessfulDelete;
                return RedirectToAction("clientindex");
            }

            TempData[AppConstants.AlertDialog] = LgsAlertEnums.InvalidModel;
            return RedirectToAction("clientindex");
        }

        #endregion


        #region Delete / Block Sub-Admin-User

        [HttpGet]
        [Authorize(Roles = RoleName.Admin)]
        public async Task<ActionResult> DeleteSubAdminUser(int subAdminId)
        {
            if (subAdminId > 0)
            {
                var isDeleted = await Service.DeleteSubAdminAppUser(subAdminId);
                TempData[AppConstants.AlertDialog] = LgsAlertEnums.SuccessfulDelete;
                return RedirectToAction("subadminindex");
            }

            TempData[AppConstants.AlertDialog] = LgsAlertEnums.InvalidModel;
            return RedirectToAction("subadminindex");
        }

        [HttpGet]
        [Authorize(Roles = RoleName.Admin)]
        public async Task<ActionResult> BlockSubAdminUser(int id)
        {
            if (id > 0)
            {
                var isBlocked = await Service.BlockSubAdminUser(id);
                if (isBlocked)
                {
//                    TempData[AppConstants.AlertDialog] = LgsAlertEnums.SuccessfulBlock;
                    return RedirectToAction("subadminindex");
                }
                else
                {
//                    TempData[AppConstants.AlertDialog] = LgsAlertEnums.SuccessfulUnBlock;
                    return RedirectToAction("subadminindex");
                }
            }

            TempData[AppConstants.AlertDialog] = LgsAlertEnums.InvalidModel;
            return RedirectToAction("subadminindex");
        }

        #endregion


        #region Open Live Chat Page

        public ActionResult LiveChat()
        {
            return View("livechatindex");
        }

        #endregion

        [HttpGet]
        public async Task<ActionResult> ProfilePage()
        {
            if (TempData[AppConstants.AlertDialog] == null)
                TempData[AppConstants.AlertDialog] = 0;
            ViewBag.AlertDialog = (int) TempData[AppConstants.AlertDialog];

            var loggedInUserId = User.Identity.GetUserId();
            var userVm = await Service.GetLoggedInUserInfo(loggedInUserId);
            if (userVm != null)
            {
                var userDashboardVm = new DashboardViewModel
                {
                    UserVm = userVm
                };
                return View(userDashboardVm);
            }

            return View();
        }


        #region Get Users With Roles User_View_Model Helper method

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

        #endregion
    }
}