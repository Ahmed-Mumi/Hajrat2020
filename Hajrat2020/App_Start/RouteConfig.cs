using System.Web.Mvc;
using System.Web.Routing;

namespace Hajrat2020
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Donations", action = "GetDonations", id = UrlParameter.Optional }
                //defaults: new { controller = "Account", action = "Login", id = UrlParameter.Optional }
            );
        }
    }
}
