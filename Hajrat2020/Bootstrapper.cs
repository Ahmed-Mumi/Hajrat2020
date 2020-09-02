using Hajrat2020.Controllers;
using Hajrat2020.Interfaces;
using Hajrat2020.Services;
using Microsoft.Practices.Unity;
using System.Web.Mvc;
using Unity.Mvc3;

namespace Hajrat2020
{
    public static class Bootstrapper
    {
        public static void Initialise()
        {
            var container = BuildUnityContainer();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }

        private static IUnityContainer BuildUnityContainer()
        {
            var container = new UnityContainer();

            container.RegisterType<IDonationService, DonationService>();
            container.RegisterType<IController, DonationsController>("Donations");
            container.RegisterType<IUserService, UserService>();
            container.RegisterType<IController, UserController>("User");
            container.RegisterType<IFamilyInNeedService, FamilyInNeedService>();
            container.RegisterType<IController, FamilyInNeedController>("FamilyInNeed");
            container.RegisterType<AccountController>(new InjectionConstructor());
            //container.RegisterType<DonationsController>(new InjectionConstructor());


            return container;
        }
    }
}