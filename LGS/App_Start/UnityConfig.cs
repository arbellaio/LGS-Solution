using System.Web.Mvc;
using LGS.Controllers;
using LGS.Controllers.Admin;
using LGS.Data;
using LGS.Data.Services.UserServices;
using LGS.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Unity;
using Unity.Injection;
using Unity.Mvc5;

namespace LGS
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();
//            container.RegisterType(typeof(IUserStore<ApplicationUser>), typeof(UserStore<ApplicationUser>));
//            container.RegisterType<ApplicationSignInManager>();
//            container.RegisterType<ApplicationUserManager>();
//            container.RegisterType<ApplicationDbContext>();
              container.RegisterType<AccountController>(new InjectionConstructor());
              container.RegisterType<AdminController>(new InjectionConstructor());
              container.RegisterType<IAdminService,AdminService>();


            DependencyResolver.SetResolver(new UnityDependencyResolver(container));

        }
    }
}