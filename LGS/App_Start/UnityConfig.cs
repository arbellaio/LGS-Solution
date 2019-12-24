using System.Web.Mvc;
using LGS.Controllers;
using LGS.Controllers.Admin;
using LGS.Controllers.Client;
using LGS.Data;
using LGS.Data.Services.ClientServices;
using LGS.Data.Services.HomeServices;
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

              container.RegisterType<AccountController>(new InjectionConstructor());
              container.RegisterType<AdminController>(new InjectionConstructor());
              container.RegisterType<ClientController>(new InjectionConstructor());
//              container.RegisterType<HomeController>(new InjectionConstructor());
              container.RegisterType<IAdminService,AdminService>();
              container.RegisterType<IClientService,ClientService>();
              container.RegisterType<IHomeService, HomeService>();



            DependencyResolver.SetResolver(new UnityDependencyResolver(container));

        }
    }
}