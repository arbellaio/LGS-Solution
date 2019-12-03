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
using LGS.Helpers.IFrameGen;
using LGS.Helpers.Invoices;
using LGS.Helpers.PaypalConfig;
using LGS.Models.Companies;
using LGS.Models.Credits;
using LGS.Models.RoleNames;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using PayPal.Api;
using DashboardViewModel = LGS.Models.ViewModels.DashboardViewModels.DashboardViewModel;

namespace LGS.Controllers.Client
{
    [HandleError]
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
            }

            TempData[AppConstants.AlertDialog] = LgsAlertEnums.InvalidModel;
            return RedirectToAction("companydetail", "client",
                new {id = companyViewModel.Company.Id, clientId = companyViewModel.Company.ClientId});
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
            ViewBag.AlertDialog = (int) TempData[AppConstants.AlertDialog];
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


        #region Cart And Purchase With Paypal

        [HttpPost]
        public ActionResult Cart(Models.PaypalItem.Item item)
        {
            if (item != null && !item.quantity.Equals("0"))
            {
                var items = new List<Models.PaypalItem.Item> {item};
                var dashboardVm = new DashboardViewModel
                {
                    Items = items,
                };
                return View(dashboardVm);
            }

            return RedirectToAction("index");
        }

        public ActionResult PaymentWithPaypal(List<Models.PaypalItem.Item> Items)
        {
            if (Items != null && Items.Any())
            {
                var apiContext = PaypalConfiguration.GetAPIContext();
                (Payment, CreditInvoice, AccountCredit) paymentInfo = default;
                try
                {
                    string payerId = Request.Params[AppConstants.PayerId];
                    if (string.IsNullOrEmpty(payerId))
                    {
                        string baseURI = Request.Url.Scheme + AppConstants.ColonDoubleForwardSlash +
                                         Request.Url.Authority + AppConstants.ControllerPaymentWithPal;

                        var guid = Convert.ToString((new Random()).Next(100000));
                        paymentInfo = this.CreatePayment(apiContext, baseURI + AppConstants.Guid_equal + guid, Items);

                        var links = paymentInfo.Item1.links.GetEnumerator();
                        string paypalRedirectUrl = null;

                        while (links.MoveNext())
                        {
                            Links lnk = links.Current;
                            if (lnk.rel.ToLower().Trim().Equals(AppConstants.ApprovedUrl))
                            {
                                paypalRedirectUrl = lnk.href;
                            }
                        }

                        Session.Add(guid, paymentInfo.Item1.id);

                        if (paymentInfo.Item2 != null && paymentInfo.Item3 != null)
                        {
                            var isSaleCompleted = Service.AddInvoice(paymentInfo.Item2, paymentInfo.Item3);
                        }

                        return Redirect(paypalRedirectUrl);
                    }
                    else
                    {
                        var guid = Request.Params[AppConstants.Guid];
                        var executedPayment = ExecutePayment(apiContext, payerId, Session[guid] as string);

                        if (executedPayment.state.ToLower() != AppConstants.Approved)
                        {
                            return View("FailureView");
                        }
                    }
                }
                catch (Exception ex)
                {
                    return View("FailureView");
                }

                if (paymentInfo.Item2 != null && paymentInfo.Item3 != null)
                {
                    var isSaleCompleted = Service.AddInvoice(paymentInfo.Item2, paymentInfo.Item3);
                }


                return View("SuccessView");
            }

            return RedirectToAction("index");
        }

        private PayPal.Api.Payment payment;

        private Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution()
            {
                payer_id = payerId
            };
            this.payment = new Payment()
            {
                id = paymentId
            };
            return this.payment.Execute(apiContext, paymentExecution);
        }

        private (Payment, CreditInvoice, AccountCredit) CreatePayment(APIContext apiContext, string redirectUrl,
            List<Models.PaypalItem.Item> cartItems)
        {
            var cartItemsPaypal = GetCartInfo(cartItems);
            var itemList = new ItemList()
            {
                items = cartItemsPaypal
            };

            var payer = new Payer()
            {
                payment_method = AppConstants.PayerPaymentMethod
            };

            var redirUrls = new RedirectUrls()
            {
                cancel_url = redirectUrl + AppConstants.AndCancel,
                return_url = redirectUrl
            };

            var details = new Details()
            {
                subtotal = InvoiceHelper.GetStringItemsSum(itemList.items)
            };

            var amountC = new Amount()
            {
                currency = AppConstants.Currency,
                total = InvoiceHelper.GetStringItemsSum(itemList.items),
                details = details
            };

            var transactionList = new List<Transaction>();
            transactionList.Add(new Transaction()
            {
                description = AppConstants.DescriptionPurchase,
                invoice_number =
                    InvoiceHelper.GetInvoiceNumber(
                        InvoiceHelper.GetUserNameFromEmail(cartItems[0].Email)), //Generate an Invoice No  
                amount = amountC,
                item_list = itemList
            });

            this.payment = new Payment()
            {
                intent = AppConstants.PaymentIntent,
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrls
            };

            var creditInvoice = new CreditInvoice
            {
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow,
                InvoiceNo = transactionList[0].invoice_number,
                TransactionAmount = Convert.ToDecimal(transactionList[0].amount.total),
                TransactionId = Guid.NewGuid().ToString(),
                UserId = cartItems[0].UserId,
                ClientId = cartItems[0].ClientId,
                Description = cartItems[0].description,
                Name = cartItems[0].name,
                SkuCode = cartItems[0].sku,
                Quantity = cartItems[0].quantity,
                Currency = cartItems[0].currency,
                Price = cartItems[0].priceperitem,
                TotalPrice = cartItems[0].price,
                Tax = cartItems[0].tax,
                Url = cartItems[0].url,
                TransactionService = AppConstants.PayerPaymentMethod
            };
            var accountCredit = new AccountCredit
            {
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow,
                ClientId = cartItems[0].ClientId,
                TotalCredits = Convert.ToDecimal(itemList.items[0].quantity),
                InvoiceNo = creditInvoice.InvoiceNo,
                TransactionId = creditInvoice.TransactionId,
                UserId = cartItems[0].UserId
            };
            // Create a payment using a APIContext  
            return (this.payment.Create(apiContext), creditInvoice, accountCredit);
        }

        public List<Item> GetCartInfo(List<Models.PaypalItem.Item> _itemsToBuy)
        {
            var cartItems = new List<Item>();
            if (_itemsToBuy != null)
            {
                foreach (var item in _itemsToBuy)
                {
                    var cartItem = new Item
                    {
                        sku = item.sku,
                        quantity = item.quantity,
                        currency = AppConstants.Currency,
                        description = item.description,
                        name = item.name,
                        price = item.price,
                    };
                    cartItems.Add(cartItem);
                }

                return cartItems;
            }

            return null;
        }

        #endregion

        #region Account Invoice Details

        [HttpGet]
        public async Task<ActionResult> GetInvoiceDetails(int id)
        {
            if (id == 0) return Json("");
            var creditInvoice = await Service.GetInvoiceDetails(id);
            return Json(creditInvoice, JsonRequestBehavior.AllowGet);
        }

        #endregion

        public async Task<ActionResult> BuyServices(Models.PaypalItem.Item item)
        {
            var accountCredit = await Service.GetAccountCredit(item.ClientId);
            var companyInventoryDb = await Service.GetCompanyInventory(item.CompanyId);
            if (accountCredit != null)
            {
                accountCredit.AvailableCredits -= Convert.ToDecimal(item.price);
//                var invoice = new CreditInvoice
//                {
//                    ClientId = item.ClientId,
//                    CreatedDate = DateTime.UtcNow,
//                    UpdatedDate = DateTime.UtcNow,
//                    TransactionAmount = Convert.ToDecimal(item.price),
//                    Description = item.description,
//                    InvoiceNo = InvoiceHelper.GetInvoiceNumber(InvoiceHelper.GetUserNameFromEmail(item.Email)),
//                    Currency = "Cr",
//                    Name = item.name,
//                    Quantity = item.quantity,
//                    SkuCode = item.sku,
//                    Price = item.priceperitem,
//                    TotalPrice = item.price,
//                    TransactionService = AppConstants.DescriptionPurchaseInternal,
//                    TransactionId = Guid.NewGuid().ToString(),
//                    UserId = item.UserId,
//                };

                var companyInvoice = new CompanyInvoice
                {
                    ClientId = item.ClientId,
                    CompanyId = item.CompanyId,
                    CreatedDate = DateTime.UtcNow,
                    UpdatedDate = DateTime.UtcNow,
                    Credits = Convert.ToDecimal(item.price),
                    InvoiceNo = InvoiceHelper.GetInvoiceNumber(InvoiceHelper.GetUserNameFromEmail(item.Email)),
                    ItemName = item.name,
                    ItemQuantity = Convert.ToInt32(item.quantity),
                    UserId = item.UserId,
                    ItemSkuCode = item.sku,
                };


                if (companyInventoryDb == null)
                {
                    companyInventoryDb = new CompanyInventory()
                    {
                        CompanyId = item.CompanyId,
                        CreatedDate = DateTime.UtcNow,
                        UpdatedDate = DateTime.UtcNow,
                        ItemName = item.name,
                        TotalItemBought = Convert.ToInt32(item.quantity),
                        RemainingItems = Convert.ToInt32(item.quantity),
                        UserId = item.UserId,
                        ItemSkuCode = item.sku,
                    };
                }
                else
                {
                    companyInventoryDb.CompanyId = item.CompanyId;
                    companyInventoryDb.CreatedDate = DateTime.UtcNow;
                    companyInventoryDb.UpdatedDate = DateTime.UtcNow;
                    companyInventoryDb.ItemName = item.name;
                    companyInventoryDb.TotalItemBought = Convert.ToInt32(item.quantity);
                    companyInventoryDb.RemainingItems = Convert.ToInt32(item.quantity);
                    companyInventoryDb.UserId = item.UserId;
                    companyInventoryDb.ItemSkuCode = item.sku;
                }


                await Service.AddCompanyPurchase(accountCredit, companyInvoice, companyInventoryDb);
                return RedirectToAction("companydetail", "client",
                    new {id = item.CompanyId, clientId = item.ClientId});
            }

            return null;
        }

        public async Task SetSettings(CompanyViewModel companyViewModel)
        {
            if (companyViewModel != null && companyViewModel.CompanyId > 0)
            {
                var company = new Company
                {
                    DeliveryInterval = companyViewModel.RadioInterval,
                    LeadLimit = companyViewModel.LeadQuantity,
                    NotificationMode = companyViewModel.RadioNotification,
                    Id = companyViewModel.CompanyId,
                };
                var isSaved = await Service.SaveCompanySettings(company);
                if (isSaved)
                {
                    TempData[AppConstants.AlertDialog] = LgsAlertEnums.SavedSettings;
                    return;
//                    return RedirectToAction("companydetail", "client",new {id = companyViewModel.CompanyId, clientId = companyViewModel.ClientId});
                }

                TempData[AppConstants.AlertDialog] = LgsAlertEnums.SaveSettingsFailed;
//                return RedirectToAction("companydetail", "client",
//                    new {id = companyViewModel.CompanyId, clientId = companyViewModel.ClientId});
            }

            TempData[AppConstants.AlertDialog] = LgsAlertEnums.SaveSettingsFailed;
//            return RedirectToAction("companydetail", "client",
//                new {id = companyViewModel.CompanyId, clientId = companyViewModel.ClientId});
        }

        public async Task<JsonResult> GetCustomerReviews(string email)
        {
            if (!string.IsNullOrEmpty(email))
            {
                var reviews = await Service.GetCustomerReviews(email);
                return Json(reviews, JsonRequestBehavior.AllowGet);
            }
            return null;
        }

        public async Task<JsonResult> GetCustomerMessages(int id)
        {
            if (id > 0)
            {
                var message = await Service.GetCustomerMessage(id);
                var mapLocation = FrameGenerator.GenerateIFrame(message.AddressOneUnit + " " + message.AddressTwoStreet + " "+ message.AddressThreeLocality);
                message.MapLocation = mapLocation;
                return Json(message, JsonRequestBehavior.AllowGet);
            }
            return null;
        }
    }
}