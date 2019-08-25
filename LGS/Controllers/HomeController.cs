using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using KruisIT.Web.Analytics.Attributes;
using LGS.AppProperties;
using LGS.Data.Services.UserServices;
using LGS.Models.RoleNames;

namespace LGS.Controllers
{
    [Analytics(AppConstants.ConnectionName)]
    public class HomeController : Controller
    {
        private readonly IAdminService _service;

        public HomeController(IAdminService service)
        {
            _service = service ?? throw new NullReferenceException("User Service");
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
                return RedirectToAction("index", "admin");

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
    }
}