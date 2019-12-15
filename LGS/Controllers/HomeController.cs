using KruisIT.Web.Analytics.Attributes;
using LGS.AppProperties;
using LGS.Data;
using LGS.Data.Services.HomeServices;
using LGS.Helpers.Ratings;
using LGS.Models.Communication;
using LGS.Models.RoleNames;
using LGS.Models.Users;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using LGS.Extensions;
using LGS.Helpers.TwilioHelper;
using CompanyViewModel = LGS.Models.ViewModels.DashboardViewModels.CompanyViewModel;

namespace LGS.Controllers
{
    [HandleError]
    [Analytics(AppConstants.ConnectionName)]
    public class HomeController : Controller
    {
        private IHomeService _service;

        public HomeController()
        {
        }
        public HomeController(IHomeService service)
        {
            Service = service ?? throw new NullReferenceException("User Service");
        }

        public IHomeService Service
        {
            get { return _service = new HomeService(new ApplicationDbContext()); }
            private set { _service = value; }
        }
        public ActionResult Index()
        {
            var remoteIpAddress = Request.UserHostAddress;
            if (User.IsInRole(RoleName.Admin))
            {
                return RedirectToAction("index","admin");
            }
            if (User.IsInRole(RoleName.SubAdmin))
            {
                return RedirectToAction("index", "admin");

            }
            if (User.IsInRole(RoleName.Client))
            {
                return RedirectToAction("index", "client");

            }
           
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public async Task<ActionResult> Companies()
        {
            if (TempData[AppConstants.AlertDialog] == null)
                TempData[AppConstants.AlertDialog] = 0;
            ViewBag.AlertDialog = (int)TempData[AppConstants.AlertDialog];

            var companies = await Service.GetAllCompanies();
            var companyViewModel = new CompanyViewModel
            {
                Companies = companies,
            };
            return View(companyViewModel);
        }

        public async Task<ActionResult> CompanyDetail(int id)
        {
            if (TempData[AppConstants.AlertDialog] == null)
                TempData[AppConstants.AlertDialog] = 0;
            ViewBag.AlertDialog = (int)TempData[AppConstants.AlertDialog];

            if (id > 0)
            {
                var companyInDb = await Service.GetCompanyDetailByCompanyId(id);
                if (companyInDb != null)
                {
                    var rating = RatingsHelper.CalculateRating(companyInDb.CompanyRatings);
                    companyInDb.Ratings = rating;
                    var companyDetailViewModel = new CompanyViewModel
                    {
                        Company = companyInDb,
                        
                    };
                    ViewData["Reviews"] = companyInDb.CustomerReviews.ToArray();
                    return View(companyDetailViewModel);
                }

                return RedirectToAction("companies", "home");
            }

            return RedirectToAction("companies", "home");
        }

        public async Task SetRating(float rating, int companyId)
        {
            await Service.SetCompanyRating(rating, companyId);
        }


        public async Task<ActionResult> SendMessage(CompanyViewModel companyViewModel)
        {
            if (companyViewModel != null && companyViewModel.Customer != null && companyViewModel.CompanyId != 0 && companyViewModel.CustomerMessage != null)
            {
                var customer = new Customer
                {
                    AddressOneUnit = companyViewModel.Customer.AddressOneUnit,
                    AddressThreeLocality = companyViewModel.Customer.AddressThreeLocality,
                    AddressTwoStreet = companyViewModel.Customer.AddressTwoStreet,
                    Email = companyViewModel.Customer.Email,
                    FullName = companyViewModel.Customer.FullName,
                    PhoneNumber = companyViewModel.Customer.PhoneNumber,
                    PostalCode = companyViewModel.Customer.PostalCode,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                    CompanyId = companyViewModel.CompanyId,

                };
                var customerMessage = new CustomerMessage
                {
                    CustomerEmail  = companyViewModel.Customer.Email,
                    CustomerFullName  = companyViewModel.Customer.FullName,
                    CompanyId = companyViewModel.CompanyId,
                    CustomerPhoneNumber = companyViewModel.Customer.PhoneNumber,
                    Message = companyViewModel.CustomerMessage.Message,
                    AddressOneUnit = companyViewModel.Customer.AddressOneUnit,
                    AddressThreeLocality = companyViewModel.Customer.AddressThreeLocality,
                    AddressTwoStreet = companyViewModel.Customer.AddressTwoStreet,
                    PostalCode = companyViewModel.Customer.PostalCode,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                    
                };
                var isSendMessage = await Service.SendMessage(customer,customerMessage);

                if (companyViewModel.Company.NotificationMode.Equals(LgsNotificationEnum.Email))
                {
                    await EmailSmsHelper.SendEmail(companyViewModel.Customer.Email, companyViewModel.Customer.FullName,
                        companyViewModel.Company.CompanyEmail, companyViewModel.Company.CompanyName,
                        companyViewModel.CustomerMessage.Message, companyViewModel.CustomerMessage.Message,"");
                }
                if (companyViewModel.Company.NotificationMode.Equals(LgsNotificationEnum.Sms))
                {
                    await EmailSmsHelper.SendSms(
                        companyViewModel.CustomerMessage.CustomerEmail + " " +
                        companyViewModel.CustomerMessage.CustomerFullName +"Sent Message :"+companyViewModel.CustomerMessage.Message, companyViewModel.Company.PhoneNumber);
                }
                if (companyViewModel.Company.NotificationMode.Equals(LgsNotificationEnum.Both))
                {
                    await EmailSmsHelper.SendEmail(companyViewModel.Customer.Email, companyViewModel.Customer.FullName,
                        companyViewModel.Company.CompanyEmail, companyViewModel.Company.CompanyName,
                        companyViewModel.CustomerMessage.Message, companyViewModel.CustomerMessage.Message, "");

                    await EmailSmsHelper.SendSms(
                        companyViewModel.CustomerMessage.CustomerEmail + " " +
                        companyViewModel.CustomerMessage.CustomerFullName + "Sent Message :" + companyViewModel.CustomerMessage.Message, companyViewModel.Company.PhoneNumber);
                }
                if (isSendMessage)
                {
                    TempData[AppConstants.AlertDialog] = LgsAlertEnums.MessageSent;
                    return RedirectToAction("companies");
                }
                TempData[AppConstants.AlertDialog] = LgsAlertEnums.MessageSentFailed;
                return RedirectToAction("companies");
            }
            TempData[AppConstants.AlertDialog] = LgsAlertEnums.MessageSentFailed;
            return RedirectToAction("companies");
        }

        public async Task<ActionResult> PostReview(CompanyViewModel companyViewModel)
        {
            if (companyViewModel != null && companyViewModel.CustomerReview != null)
            {
                var newCustomer = new Customer
                {
                    FullName = companyViewModel.CustomerReview.CustomerName,
                    AddressOneUnit = companyViewModel.CustomerReview.CustomerAddress,
                    AddressTwoStreet = companyViewModel.CustomerReview.CustomerAddress,
                    AddressThreeLocality = companyViewModel.CustomerReview.CustomerAddress,
                    CompanyId = companyViewModel.CustomerReview.CompanyId,
                    PhoneNumber = companyViewModel.CustomerReview.CustomerPhoneNumber,
                    Email = companyViewModel.CustomerReview.CustomerEmail,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                };
                companyViewModel.CustomerReview.CreatedDate = DateTime.Now;
                companyViewModel.CustomerReview.UpdatedDate = DateTime.Now;
                var IsSaved = await Service.SendReview(newCustomer, companyViewModel.CustomerReview);
                if (IsSaved)
                {
                    TempData[AppConstants.AlertDialog] = LgsAlertEnums.ReviewSaved;
                    return RedirectToAction("companydetail", "home", new { id = companyViewModel.CustomerReview.CompanyId });
                }
                TempData[AppConstants.AlertDialog] = LgsAlertEnums.ReviewSaveFailed;
                return RedirectToAction("companydetail", "home", new { id = companyViewModel.CustomerReview.CompanyId });
            }
            TempData[AppConstants.AlertDialog] = LgsAlertEnums.ReviewSaveFailed;
            return RedirectToAction("companies");
        }
    }
}