using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using KruisIT.Web.Analytics.Attributes;
using LGS.AppProperties;
using LGS.Data;
using LGS.Data.Services.HomeServices;
using LGS.Data.Services.UserServices;
using LGS.Data.ViewModels.DatabaseViewModels;
using LGS.Helpers.Ratings;
using LGS.Models.RoleNames;
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
            var companies = await Service.GetAllCompanies();
            var companyViewModel = new CompanyViewModel
            {
                Companies = companies,
            };
            return View(companyViewModel);
        }

        public async Task<ActionResult> CompanyDetail(int id)
        {
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
    }
}